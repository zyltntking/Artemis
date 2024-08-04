using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Protos;
using Artemis.Service.Protos.School;
using Artemis.Service.School.Managers;
using Artemis.Service.Shared.School.Transfer;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Services;

/// <summary>
///     教师服务实现
/// </summary>
public class TeacherServiceImplement : TeacherService.TeacherServiceBase
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="schoolTeacherManager"></param>
    /// <param name="logger"></param>
    public TeacherServiceImplement(
        ISchoolTeacherManager schoolTeacherManager,
        ILogger<TeacherServiceImplement> logger)
    {
        SchoolTeacherManager = schoolTeacherManager;
        Logger = logger;
    }

    /// <summary>
    ///     教师管理器
    /// </summary>
    private ISchoolTeacherManager SchoolTeacherManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<TeacherServiceImplement> Logger { get; }

    #region Overrides of TeacherServiceBase

    /// <summary>
    ///     查询教师信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询教师信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchTeacherInfoResponse> SearchTeacherInfo(SearchTeacherInfoRequest request,
        ServerCallContext context)
    {
        var info = await SchoolTeacherManager.FetchTeachersAsync(
            request.Name,
            request.Code,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchTeacherInfoResponse, TeacherInfo>();
    }

    /// <summary>
    ///     获取教师信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取教师信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadTeacherInfoResponse> ReadTeacherInfo(ReadTeacherInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.TeacherId);

        var info = await SchoolTeacherManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadTeacherInfoResponse, TeacherInfo>();
    }

    /// <summary>
    ///     创建教师
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建教师")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateTeacher(CreateTeacherRequest request, ServerCallContext context)
    {
        var package = request.Adapt<TeacherPackage>();

        var result = await SchoolTeacherManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建教师
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建教师")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateTeachers(BatchCreateTeacherRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<TeacherPackage>>();

        var result = await SchoolTeacherManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新教师
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新教师")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateTeacher(UpdateTeacherRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.TeacherId);

        var package = request.Teacher.Adapt<TeacherPackage>();

        var result = await SchoolTeacherManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新教师
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新教师")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateTeacher(BatchUpdateTeacherRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.TeacherId),
            item => item.Teacher.Adapt<TeacherPackage>());

        var result = await SchoolTeacherManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除教师
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除教师")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteTeacher(DeleteTeacherRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.TeacherId);

        var result = await SchoolTeacherManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除教师
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除教师")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteTeacher(BatchDeleteTeacherRequest request,
        ServerCallContext context)
    {
        var ids = request.TeacherIds.Select(Guid.Parse);

        var result = await SchoolTeacherManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}