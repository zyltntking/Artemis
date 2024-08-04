using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.School.Context;
using Artemis.Service.School.Stores;
using Artemis.Service.Shared.School.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Managers;

/// <summary>
///     学校学生管理器接口
/// </summary>
public interface ISchoolStudentManager :
    IOptionalOneToManySubManager<ArtemisSchool, ArtemisStudent, StudentInfo, StudentPackage>
{
    /// <summary>
    ///     根据学生信息查找学生
    /// </summary>
    /// <param name="studentNameSearch">学生姓名搜索值</param>
    /// <param name="studentNumberSearch">学生学籍号搜索值</param>
    /// <param name="gender">学生性别</param>
    /// <param name="nation">学生民族</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<StudentInfo>> FetchStudentsAsync(
        string? studentNameSearch,
        string? studentNumberSearch,
        string? gender,
        string? nation,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
///     学校学生管理器实现
/// </summary>
public class SchoolStudentManager :
    OptionalOneToManySubManager<ArtemisSchool, ArtemisStudent, StudentInfo, StudentPackage>,
    ISchoolStudentManager
{
    /// <summary>
    ///     可选的一对多模型管理器构造
    /// </summary>
    public SchoolStudentManager(
        IArtemisSchoolStore schoolStore,
        IArtemisStudentStore studentStore) : base(schoolStore, studentStore)
    {
    }

    #region Implementation of ISchoolStudentManager

    /// <summary>
    ///     根据学生信息查找学生
    /// </summary>
    /// <param name="studentNameSearch">学生姓名搜索值</param>
    /// <param name="studentNumberSearch">学生学籍号搜索值</param>
    /// <param name="gender">学生性别</param>
    /// <param name="nation">学生民族</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<StudentInfo>> FetchStudentsAsync(
        string? studentNameSearch,
        string? studentNumberSearch,
        string? gender,
        string? nation,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        studentNameSearch ??= string.Empty;
        studentNumberSearch ??= string.Empty;
        gender ??= string.Empty;
        nation ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            studentNameSearch != string.Empty,
            student => EF.Functions.Like(
                student.Name, $"%{studentNameSearch}%"));

        query = query.WhereIf(
            studentNumberSearch != string.Empty,
            student => EF.Functions.Like(
                student.StudentNumber, $"%{studentNumberSearch}%"));

        query = query.WhereIf(gender != string.Empty, student => student.Gender == gender);

        query = query.WhereIf(nation != string.Empty, student => student.Nation == nation);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(teacher => teacher.Name);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var students = await query
            .ProjectToType<StudentInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<StudentInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = students
        };
    }

    #endregion

    #region Overrides

    /// <summary>
    ///     设置模型的关联键
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="mainKey"></param>
    protected override void SetSubEntityRelationalKey(ArtemisStudent entity, Guid mainKey)
    {
        entity.SchoolId = mainKey;
    }

    /// <summary>
    ///     移除模型的关联键
    /// </summary>
    /// <param name="entity"></param>
    protected override void RemoveEntityRelationalKey(ArtemisStudent entity)
    {
        entity.SchoolId = null;
    }

    #endregion
}