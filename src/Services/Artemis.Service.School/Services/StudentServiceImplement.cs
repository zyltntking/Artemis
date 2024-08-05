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
///     学生服务实现
/// </summary>
public class StudentServiceImplement : StudentService.StudentServiceBase
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="schoolStudentManager"></param>
    /// <param name="logger"></param>
    public StudentServiceImplement(
        ISchoolStudentManager schoolStudentManager,
        ILogger<StudentServiceImplement> logger)
    {
        SchoolStudentManager = schoolStudentManager;
        Logger = logger;
    }

    /// <summary>
    ///     教师管理器
    /// </summary>
    private ISchoolStudentManager SchoolStudentManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<StudentServiceImplement> Logger { get; }

    #region Overrides of StudentServiceBase

    /// <summary>
    ///     查询学生信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询学生信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchStudentInfoResponse> SearchStudentInfo(SearchStudentInfoReqeust request,
        ServerCallContext context)
    {
        var info = await SchoolStudentManager.FetchStudentsAsync(
            request.Name,
            request.StudentNumber,
            request.Gender,
            request.Nation,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchStudentInfoResponse, StudentInfo>();
    }

    /// <summary>
    ///     获取学生信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取学生信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadStudentInfoResponse> ReadStudentInfo(ReadStudentInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.StudentId);

        var info = await SchoolStudentManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadStudentInfoResponse, StudentInfo>();
    }

    /// <summary>
    ///     创建学生
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建学生")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> CreateStudent(CreateStudentRequest request, ServerCallContext context)
    {
        var package = request.Adapt<StudentPackage>();

        var result = await SchoolStudentManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建学生
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建学生")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchCreateStudent(BatchCreateStudentRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<StudentPackage>>();

        var result = await SchoolStudentManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新学生
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新学生")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UpdateStudent(UpdateStudentRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StudentId);
        var package = request.Adapt<StudentPackage>();

        var result = await SchoolStudentManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新学生
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新学生")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchUpdateStudent(BatchUpdateStudentRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            item => Guid.Parse(item.StudentId),
            item => item.Student.Adapt<StudentPackage>());

        var result = await SchoolStudentManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除学生
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除学生")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> DeleteStudent(DeleteStudentRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.StudentId);

        var result = await SchoolStudentManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除学生
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除学生")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BatchDeleteStudent(BatchDeleteStudentRequest request,
        ServerCallContext context)
    {
        var ids = request.StudentIds.Select(Guid.Parse);

        var result = await SchoolStudentManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}