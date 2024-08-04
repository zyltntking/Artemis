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
///     学校教师管理器接口
/// </summary>
public interface ISchoolTeacherManager :
    IOptionalOneToManySubManager<ArtemisSchool, ArtemisTeacher, TeacherInfo, TeacherPackage>
{
    /// <summary>
    ///     根据教师信息查找教师
    /// </summary>
    /// <param name="teacherNameSearch">教师名称搜索值</param>
    /// <param name="teacherCodeSearch">教师编号搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    Task<PageResult<TeacherInfo>> FetchTeachersAsync(
        string? teacherNameSearch,
        string? teacherCodeSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
///     学校教师管理器实现
/// </summary>
public class SchoolTeacherManager :
    OptionalOneToManySubManager<ArtemisSchool, ArtemisTeacher, TeacherInfo, TeacherPackage>,
    ISchoolTeacherManager
{
    /// <summary>
    ///     可选的一对多模型管理器构造
    /// </summary>
    public SchoolTeacherManager(
        IArtemisSchoolStore schoolStore,
        IArtemisTeacherStore teacherStore) : base(schoolStore, teacherStore)
    {
    }

    #region Implementation of ISchoolTeacherManager

    /// <summary>
    ///     根据教师信息查找教师
    /// </summary>
    /// <param name="teacherNameSearch">教师名称搜索值</param>
    /// <param name="teacherCodeSearch">教师编号搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>分页搜索结果</returns>
    public async Task<PageResult<TeacherInfo>> FetchTeachersAsync(
        string? teacherNameSearch,
        string? teacherCodeSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        teacherNameSearch ??= string.Empty;
        teacherCodeSearch ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            teacherNameSearch != string.Empty,
            teacher => EF.Functions.Like(
                teacher.Name, $"%{teacherNameSearch}%"));

        query = query.WhereIf(
            teacherCodeSearch != string.Empty,
            teacher => EF.Functions.Like(
                teacher.Code, $"%{teacherCodeSearch}%"));

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(teacher => teacher.Name);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var teachers = await query
            .ProjectToType<TeacherInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<TeacherInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = teachers
        };
    }

    #endregion

    #region Overrides

    /// <summary>
    ///     设置模型的关联键
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="mainKey"></param>
    protected override void SetSubEntityRelationalKey(ArtemisTeacher entity, Guid mainKey)
    {
        entity.SchoolId = mainKey;
    }

    /// <summary>
    ///     移除模型的关联键
    /// </summary>
    /// <param name="entity"></param>
    protected override void RemoveEntityRelationalKey(ArtemisTeacher entity)
    {
        entity.SchoolId = null;
    }

    #endregion
}