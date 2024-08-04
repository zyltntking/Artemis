using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Protos;
using Artemis.Service.Protos.School;
using Artemis.Service.School.Managers;
using Artemis.Service.Shared;
using Artemis.Service.Shared.School.Transfer;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Services;

/// <summary>
///     学校服务实现
/// </summary>
public class SchoolServiceImplement : SchoolService.SchoolServiceBase
{
    /// <summary>
    ///     服务实现
    /// </summary>
    /// <param name="schoolManager"></param>
    /// <param name="logger"></param>
    public SchoolServiceImplement(
        ISchoolManager schoolManager,
        ILogger<SchoolServiceImplement> logger)
    {
        SchoolManager = schoolManager;
        Logger = logger;
    }

    /// <summary>
    ///     学校管理器
    /// </summary>
    private ISchoolManager SchoolManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<SchoolServiceImplement> Logger { get; }

    #region Overrides of SchoolServiceBase

    /// <summary>
    ///     查询学校信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询学校信息")]
    //[Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchSchoolInfoResponse> SearchSchoolInfo(SearchSchoolReqeust request,
        ServerCallContext context)
    {
        var info = await SchoolManager.FetchSchoolsAsync(
            request.Name,
            request.Code,
            request.OrganizationCode,
            request.DivisionCode,
            request.Type,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchSchoolInfoResponse, SchoolInfo>();
    }

    /// <summary>
    ///     读取学校信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("读取学校信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadSchoolInfoResponse> ReadSchoolInfo(ReadSchoolInfoRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var info = await SchoolManager.ReadEntityInfoAsync(id, context.CancellationToken);

        return info.ReadInfoResponse<ReadSchoolInfoResponse, SchoolInfo>();
    }

    /// <summary>
    ///     创建学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建学校")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateSchool(CreateSchoolRequest request, ServerCallContext context)
    {
        var package = request.Adapt<SchoolPackage>();

        var result = await SchoolManager.CreateEntityAsync(package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建学校")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateSchool(BatchCreateSchoolRequest request,
        ServerCallContext context)
    {
        var packages = request.Batch.Adapt<IEnumerable<SchoolPackage>>();

        var result = await SchoolManager.BatchCreateEntityAsync(packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新学校")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateSchool(UpdateSchoolRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var package = request.School.Adapt<SchoolPackage>();

        var result = await SchoolManager.UpdateEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新学校")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateSchool(BatchUpdateSchoolRequest request,
        ServerCallContext context)
    {
        var dictionary = request.Batch.ToDictionary(
            school => Guid.Parse(school.SchoolId),
            school => school.School.Adapt<SchoolPackage>());

        var result = await SchoolManager.BatchUpdateEntityAsync(dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除学校")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteSchool(DeleteSchoolRequest request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var result = await SchoolManager.DeleteEntityAsync(id, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除学校")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteSchool(BatchDeleteSchoolRequest request,
        ServerCallContext context)
    {
        var ids = request.SchoolIds.Select(Guid.Parse);

        var result = await SchoolManager.BatchDeleteEntityAsync(ids, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     查询学校班级信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询学校班级信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<SearchSchoolClassInfoResponse> SearchSchoolClassInfo(
        SearchSchoolClassInfoReqeust request, ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var info = await SchoolManager.FetchSchoolClassesAsync(
            id,
            request.Name,
            request.Code,
            request.GradeName,
            request.Type,
            request.StudyPhase,
            request.SchoolLength,
            request.Page ?? 0,
            request.Size ?? 0,
            context.CancellationToken);

        return info.PagedResponse<SearchSchoolClassInfoResponse, ClassInfo>();
    }

    /// <summary>
    ///     获取学校班级信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取学校班级信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<ReadSchoolClassInfoResponse> ReadSchoolClassInfo(ReadSchoolClassInfoRequest request,
        ServerCallContext context)
    {
        var schoolId = Guid.Parse(request.SchoolId);
        var classId = Guid.Parse(request.ClassId);

        var info = await SchoolManager.ReadSubEntityInfoAsync(schoolId, classId, context.CancellationToken);

        return info.ReadInfoResponse<ReadSchoolClassInfoResponse, ClassInfo>();
    }

    /// <summary>
    ///     创建学校班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("创建学校班级")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> CreateSchoolClass(CreateSchoolClassRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var package = request.Class.Adapt<ClassPackage>();

        var result = await SchoolManager.CreateSubEntityAsync(id, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量创建学校班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量创建学校班级")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchCreateSchoolClass(BatchCreateSchoolClassRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var packages = request.Batch.Adapt<IEnumerable<ClassPackage>>();

        var result = await SchoolManager.BatchCreateSubEntityAsync(id, packages, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     更新学校班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("更新学校班级")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> UpdateSchoolClass(UpdateSchoolClassRequest request,
        ServerCallContext context)
    {
        var schoolId = Guid.Parse(request.SchoolId);
        var classId = Guid.Parse(request.ClassId);

        var package = request.Class.Adapt<ClassPackage>();

        var result = await SchoolManager.UpdateSubEntityAsync(schoolId, classId, package, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量更新学校班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量更新学校班级")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchUpdateSchoolClass(BatchUpdateSchoolClassRequest request,
        ServerCallContext context)
    {
        var id = Guid.Parse(request.SchoolId);

        var dictionary = request.Batch.ToDictionary(
            iClass => Guid.Parse(iClass.ClassId),
            iClass => iClass.Class.Adapt<ClassPackage>());

        var result = await SchoolManager.BatchUpdateSubEntityAsync(id, dictionary, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     删除学校班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("删除学校班级")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> DeleteSchoolClass(DeleteSchoolClassRequest request,
        ServerCallContext context)
    {
        var schoolId = Guid.Parse(request.SchoolId);
        var classId = Guid.Parse(request.ClassId);

        var result = await SchoolManager.DeleteSubEntityAsync(schoolId, classId, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    ///     批量删除学校班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("批量删除学校班级")]
    [Authorize(AuthorizePolicy.Admin)]
    public override async Task<AffectedResponse> BatchDeleteSchoolClass(BatchDeleteSchoolClassRequest request,
        ServerCallContext context)
    {
        var schoolId = Guid.Parse(request.SchoolId);

        var classIds = request.ClassIds.Select(Guid.Parse);

        var result = await SchoolManager.BatchDeleteSubEntityAsync(schoolId, classIds, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}