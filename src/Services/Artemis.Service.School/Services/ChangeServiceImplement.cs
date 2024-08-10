using System.ComponentModel;
using Artemis.Extensions.Identity;
using Artemis.Service.Protos;
using Artemis.Service.Protos.School;
using Artemis.Service.School.Managers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Services;

/// <summary>
/// 变更服务
/// </summary>
public class ChangeServiceImplement : ChangeService.ChangeServiceBase
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="changeManager"></param>
    /// <param name="logger"></param>
    public ChangeServiceImplement(
        IChangeManager changeManager,
        ILogger<ChangeServiceImplement> logger)
    {
        ChangeManager = changeManager;
        Logger = logger;
    }

    /// <summary>
    ///     教师管理器
    /// </summary>
    private IChangeManager ChangeManager { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger<ChangeServiceImplement> Logger { get; }


    #region Overrides of ChangeServiceBase

    /// <summary>
    /// 教师转出学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("教师转出学校")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> SchoolTeacherMoveOut(SchoolTeacherMoveOutRequest request, ServerCallContext context)
    {
        var teacherId = Guid.Parse(request.TeacherId);

        var result =
            await ChangeManager.SchoolTeacherMoveOutAsync(teacherId, request.Reason, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 教师转入学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("教师转入学校")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> SchoolTeacherMoveIn(SchoolTeacherMoveInRequest request, ServerCallContext context)
    {
        var teacherId = Guid.Parse(request.TeacherId);

        var schoolId = Guid.Parse(request.SchoolId);

        var result =
            await ChangeManager.SchoolTeacherMoveInAsync(teacherId, schoolId, request.Reason,
                context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 学生转出学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("学生转出学校")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> SchoolStudentMoveOut(SchoolStudentMoveOutRequest request, ServerCallContext context)
    {
        var studentId = Guid.Parse(request.StudentId);

        var result =
            await ChangeManager.SchoolStudentMoveOutAsync(studentId, request.Reason, context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 学生转入学校
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("学生转入学校")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> SchoolStudentMoveIn(SchoolStudentMoveInRequest request, ServerCallContext context)
    {
        var studentId = Guid.Parse(request.StudentId);

        var schoolId = Guid.Parse(request.SchoolId);

        var result =
            await ChangeManager.SchoolStudentMoveInAsync(studentId, schoolId, request.Reason,
                context.CancellationToken);

        return result.AffectedResponse();
    }

    /// <summary>
    /// 学生变更班级
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("学生变更班级")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> StudentChangeClass(StudentChangeClassRequest request, ServerCallContext context)
    {
        var studentId = Guid.Parse(request.StudentId);

        var classId = Guid.Parse(request.ClassId);

        var result =
            await ChangeManager.StudentChangeClassAsync(studentId, classId, request.Reason, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}