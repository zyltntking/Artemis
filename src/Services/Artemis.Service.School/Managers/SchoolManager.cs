using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.School.Context;
using Artemis.Service.School.Stores;
using Artemis.Service.Shared.School.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Managers;

/// <summary>
///     学校管理器接口
/// </summary>
public interface ISchoolManager : IRequiredOneToManyManager<
    ArtemisSchool, SchoolInfo, SchoolPackage,
    ArtemisClass, ClassInfo, ClassPackage>
{
    /// <summary>
    ///     根据学校信息查找学校
    /// </summary>
    /// <param name="schoolNameSearch">学校名搜索值</param>
    /// <param name="schoolCodeSearch">学校编码搜索值</param>
    /// <param name="divisionCodeSearch">行政区划编码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="schoolType">学校类型</param>
    /// <param name="organizationCodeSearch">组织机构搜索值</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<SchoolInfo>> FetchSchoolsAsync(
        string? schoolNameSearch,
        string? schoolCodeSearch,
        string? organizationCodeSearch,
        string? divisionCodeSearch,
        string? schoolType,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据班级信息查找学校中的班级
    /// </summary>
    /// <param name="classNameSearch"></param>
    /// <param name="classCodeSearch"></param>
    /// <param name="classGradeNameSearch"></param>
    /// <param name="classSchoolLength"></param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken"></param>
    /// <param name="classType">学校类型</param>
    /// <param name="classStudyPhase"></param>
    /// <param name="id"></param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<ClassInfo>> FetchSchoolClassesAsync(
        Guid id,
        string? classNameSearch,
        string? classCodeSearch,
        string? classGradeNameSearch,
        string? classType,
        string? classStudyPhase,
        string? classSchoolLength,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
///     学校管理器
/// </summary>
public class SchoolManager : RequiredOneToManyManager<
    ArtemisSchool, SchoolInfo, SchoolPackage,
    ArtemisClass, ClassInfo, ClassPackage>, ISchoolManager
{
    /// <summary>
    ///     模型管理器构造
    /// </summary>
    public SchoolManager(
        IArtemisSchoolStore schoolStore,
        IArtemisClassStore classStore) : base(schoolStore, classStore)
    {
    }

    #region Overrides

    /// <summary>
    ///     子模型使用主模型的键匹配器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected override Expression<Func<ArtemisClass, bool>> SubEntityKeyMatcher(Guid key)
    {
        return item => item.SchoolId == key;
    }

    /// <summary>
    ///     设置子模型的关联键
    /// </summary>
    /// <param name="subEntity"></param>
    /// <param name="key"></param>
    protected override void SetSubEntityRelationalKey(ArtemisClass subEntity, Guid key)
    {
        subEntity.SchoolId = key;
    }

    #endregion

    #region Implementation of ISchoolManager

    /// <summary>
    ///     根据学校信息查找学校
    /// </summary>
    /// <param name="schoolNameSearch">学校名搜索值</param>
    /// <param name="schoolCodeSearch">学校编码搜索值</param>
    /// <param name="divisionCodeSearch">行政区划编码搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <param name="schoolType">学校类型</param>
    /// <param name="organizationCodeSearch">组织机构搜索值</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<SchoolInfo>> FetchSchoolsAsync(
        string? schoolNameSearch,
        string? schoolCodeSearch,
        string? organizationCodeSearch,
        string? divisionCodeSearch,
        string? schoolType,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        schoolNameSearch ??= string.Empty;
        schoolCodeSearch ??= string.Empty;
        organizationCodeSearch ??= string.Empty;
        divisionCodeSearch ??= string.Empty;
        schoolType ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            schoolNameSearch != string.Empty,
            school => EF.Functions.Like(
                school.Name, $"%{schoolNameSearch}%"));

        query = query.WhereIf(
            schoolCodeSearch != string.Empty,
            school => EF.Functions.Like(
                school.Code, $"%{schoolCodeSearch}%"));

        query = query.WhereIf(
            organizationCodeSearch != string.Empty,
            school => EF.Functions.Like(
                school.OrganizationCode, $"%{organizationCodeSearch}%"));

        query = query.WhereIf(
            divisionCodeSearch != string.Empty,
            school => EF.Functions.Like(
                school.DivisionCode, $"%{divisionCodeSearch}%"));

        query = query.WhereIf(schoolType != string.Empty, school => school.Type == schoolType);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(school => school.OrganizationCode);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var schools = await query
            .ProjectToType<SchoolInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<SchoolInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = schools
        };
    }

    /// <summary>
    ///     根据班级信息查找学校中的班级
    /// </summary>
    /// <param name="classNameSearch"></param>
    /// <param name="classCodeSearch"></param>
    /// <param name="classGradeNameSearch"></param>
    /// <param name="classSchoolLength"></param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken"></param>
    /// <param name="classType">学校类型</param>
    /// <param name="classStudyPhase"></param>
    /// <param name="id"></param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<ClassInfo>> FetchSchoolClassesAsync(
        Guid id,
        string? classNameSearch,
        string? classCodeSearch,
        string? classGradeNameSearch,
        string? classType,
        string? classStudyPhase,
        string? classSchoolLength,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var exists = id != default && await EntityStore.ExistsAsync(id, cancellationToken);

        if (exists)
        {
            classNameSearch ??= string.Empty;
            classCodeSearch ??= string.Empty;
            classGradeNameSearch ??= string.Empty;
            classType ??= string.Empty;
            classStudyPhase ??= string.Empty;
            classSchoolLength ??= string.Empty;

            var query = SubEntityStore.EntityQuery
                .Where(item => item.SchoolId == id);

            var total = await query.LongCountAsync(cancellationToken);

            query = query.WhereIf(
                classNameSearch != string.Empty,
                iClass => EF.Functions.Like(
                    iClass.Name,
                    $"%{classNameSearch}%"));

            query = query.WhereIf(
                classCodeSearch != string.Empty,
                iClass => EF.Functions.Like(
                    iClass.Name,
                    $"%{classCodeSearch}%"));

            query = query.WhereIf(
                classGradeNameSearch != string.Empty,
                iClass => EF.Functions.Like(
                    iClass.GradeName,
                    $"%{classGradeNameSearch}%"));

            query = query.WhereIf(classType != string.Empty, iClass => iClass.Type == classType);

            query = query.WhereIf(classStudyPhase != string.Empty, iClass => iClass.StudyPhase == classStudyPhase);

            query = query.WhereIf(classSchoolLength != string.Empty,
                iClass => iClass.SchoolLength == classSchoolLength);

            var count = await query.LongCountAsync(cancellationToken);

            query = query.OrderBy(division => division.CreateBy);

            if (page > 0 && size > 0) query = query.Page(page, size);

            var classes = await query
                .ProjectToType<ClassInfo>()
                .ToListAsync(cancellationToken);

            return new PageResult<ClassInfo>
            {
                Page = page,
                Size = size,
                Count = count,
                Total = total,
                Items = classes
            };
        }

        throw new EntityNotFoundException(nameof(ArtemisSchool), id.GuidToString());
    }

    #endregion
}