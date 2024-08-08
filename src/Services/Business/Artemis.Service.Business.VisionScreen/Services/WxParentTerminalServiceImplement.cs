using System.ComponentModel;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
using Artemis.Extensions.Identity;
using Artemis.Service.Business.VisionScreen.Context;
using Artemis.Service.Business.VisionScreen.Models;
using Artemis.Service.Business.VisionScreen.Stores;
using Artemis.Service.Identity.Stores;
using Artemis.Service.Protos;
using Artemis.Service.Protos.Business.VisionScreen;
using Artemis.Service.School.Stores;
using Artemis.Service.Shared.Business.VisionScreen.Transfer;
using Artemis.Service.Shared.School.Transfer;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Services;

/// <summary>
/// 家长端服务实现
/// </summary>
public class WxParentTerminalServiceImplement : WxParentTerminalService.WxParentTerminalServiceBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userStore"></param>
    /// <param name="schoolStore"></param>
    /// <param name="studentStore"></param>
    /// <param name="studentEyePhotoStore"></param>
    /// <param name="studentRelationBindingStore"></param>
    /// <param name="visionScreenRecordStore"></param>
    public WxParentTerminalServiceImplement(
        IIdentityUserStore userStore, 
        IArtemisSchoolStore schoolStore,
        IArtemisStudentStore studentStore,
        IArtemisStudentEyePhotoStore studentEyePhotoStore,
        IArtemisStudentRelationBindingStore studentRelationBindingStore,
        IArtemisVisionScreenRecordStore visionScreenRecordStore)
    {
        UserStore = userStore;
        SchoolStore = schoolStore;
        StudentStore = studentStore;
        StudentEyePhotoStore = studentEyePhotoStore;
        StudentRelationBindingStore = studentRelationBindingStore;
        VisionScreenRecordStore = visionScreenRecordStore;
    }

    /// <summary>
    /// 用户存储
    /// </summary>
    private IIdentityUserStore UserStore { get; }

    /// <summary>
    /// 学校存储
    /// </summary>
    private IArtemisSchoolStore SchoolStore { get; }

    /// <summary>
    /// 学生存储
    /// </summary>
    private IArtemisStudentStore StudentStore { get; }

    /// <summary>
    /// 学生眼部照片存储
    /// </summary>
    private IArtemisStudentEyePhotoStore StudentEyePhotoStore { get; }

    /// <summary>
    /// 用户学生亲属关系绑定存储
    /// </summary>
    private IArtemisStudentRelationBindingStore StudentRelationBindingStore { get; }

    /// <summary>
    /// 视力筛查档案存储
    /// </summary>
    private IArtemisVisionScreenRecordStore VisionScreenRecordStore { get; }

    #region Overrides of WxParentTerminalServiceBase

    /// <summary>
    /// 获取个人中心孩子信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取个人中心孩子信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchChildInfoResponse> FetchChildrenInfo(EmptyRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<FetchChildInfoResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchChildInfoResponse>("用户不存在");
        }

        var bindings = await StudentRelationBindingStore
            .EntityQuery
            .Where(item => item.UserId == userId)
            .ProjectToType<StudentRelationBindingInfo>()
            .ToListAsync(context.CancellationToken);

        var students = await StudentStore
            .EntityQuery
            .Where(item => bindings.Select(binding => binding.StudentId).Contains(item.Id))
            .ProjectToType<StudentInfo>()
            .ToListAsync(context.CancellationToken);

        var schoolIds = students.Select(student => student.SchoolId).Distinct().ToList();

        var schools = await SchoolStore
            .EntityQuery
            .Where(item => schoolIds.Contains(item.Id))
            .ProjectToType<SchoolInfo>()
            .ToListAsync(context.CancellationToken);

        var classIds = students.Select(student => student.ClassId).Distinct().ToList();

        var classes = await SchoolStore
            .EntityQuery
            .Where(item => classIds.Contains(item.Id))
            .ProjectToType<ClassInfo>()
            .ToListAsync(context.CancellationToken);

        var studentIds = students.Select(student => student.Id).ToList();

        var studentEyePhotos = await StudentEyePhotoStore
            .EntityQuery
            .Where(item => studentIds.Contains(item.StudentId))
            .ProjectToType<StudentEyePhotoInfo>()
            .ToListAsync(context.CancellationToken);

        var recordCounts = await VisionScreenRecordStore
            .EntityQuery
            .Where(item => studentIds.Contains(item.StudentId))
            .GroupBy(item => item.StudentId)
            .Select(group => new { StudentId = group.Key, Count = group.Count() })
            .ToListAsync(context.CancellationToken);

        var childInfos = new List<ChildInfo>();

        foreach (var student in students)
        {
            var binding = bindings.FirstOrDefault(item => item.StudentId == student.Id);

            var school = schools.FirstOrDefault(item => item.Id == student.SchoolId);

            var @class = classes.FirstOrDefault(item => item.Id == student.ClassId);

            var studentEyePhoto = studentEyePhotos.FirstOrDefault(item => item.StudentId == student.Id);

            var recordCount = recordCounts.FirstOrDefault(item => item.StudentId == student.Id);

            var childInfoPacket = new ChildInfoPacket
            {
                StudentId = student.Id.ToString(),
                Name = student.Name,
                Gender = student.Gender,
                Birthday = student.Birthday.ToString(),
                Nation = student.Nation,
                StudentNumber = student.StudentNumber,
                SchoolName = school?.Name,
                ClassName = @class?.GradeName,
                Relation = binding?.Relation,
                LeftEyePhoto = studentEyePhoto?.LeftEyePhoto,
                RightEyePhoto = studentEyePhoto?.RightEyePhoto,
                BothEyePhoto = studentEyePhoto?.BothEyePhoto
            };

            var childInfo = new ChildInfo
            {
                Child = childInfoPacket,
                HistoryRecords = recordCount?.Count ?? 0
            };

            childInfos.Add(childInfo);
        }

        var response = ResultAdapter.AdaptEmptySuccess<FetchChildInfoResponse>();

        response.Data.Add(childInfos);

        return response;
    }

    /// <summary>
    /// 获取个人中心消息信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("获取个人中心消息信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<FetchParentUserMessageInfoResponse> FetchUserMessageInfo(EmptyRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<FetchParentUserMessageInfoResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<FetchParentUserMessageInfoResponse>("用户不存在");
        }




        return ResultAdapter.AdaptEmptyFail<FetchParentUserMessageInfoResponse>("用户不存在");
    }

    /// <summary>
    /// 查询孩子信息
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("查询孩子信息")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<QueryChildResponse> QueryChild(QueryChildRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<QueryChildResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<QueryChildResponse>("用户不存在");
        }

        var student = await StudentStore
            .EntityQuery
            .Where(item => item.StudentNumber == request.StudentNumber)
            .ProjectToType<StudentInfo>()
            .FirstOrDefaultAsync(context.CancellationToken);

        var school = Instance.CreateInstance<SchoolInfo>();

        if (student?.SchoolId != null)
        {
            school = await SchoolStore
                .EntityQuery
                .Where(item => item.Id == student.SchoolId)
                .ProjectToType<SchoolInfo>()
                .FirstOrDefaultAsync(context.CancellationToken);
        }

        var @class = Instance.CreateInstance<ClassInfo>();

        if (student?.ClassId != null)
        {
            @class = await SchoolStore
                .EntityQuery
                .Where(item => item.Id == student.ClassId)
                .ProjectToType<ClassInfo>()
                .FirstOrDefaultAsync(context.CancellationToken);
        }

        var childInfo = new ChildInfoPacket
        {
            StudentId = student?.Id.ToString(),
            Name = student?.Name,
            Gender = student?.Gender,
            Birthday = student?.Birthday.ToString(),
            Nation = student?.Nation,
            StudentNumber = student?.StudentNumber,
            SchoolName = school?.Name,
            ClassName = @class?.GradeName
        };

        var response = ResultAdapter.AdaptEmptySuccess<QueryChildResponse>();

        response.Data = childInfo;

        return response;
    }

    /// <summary>
    /// 绑定孩子关系
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("绑定孩子关系")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> BindChildRelation(BindChildRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户不存在");
        }

        var studentId = Guid.Parse(request.StudentId);

        var studentExists = await StudentStore.ExistsAsync(studentId, context.CancellationToken);

        if (!studentExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("学生不存在");
        }

        var bindingExists = await StudentRelationBindingStore
            .EntityQuery
            .Where(item => item.StudentId == studentId && item.Relation == request.Relation)
            .AnyAsync(context.CancellationToken);

        if (bindingExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>($"该学生已经绑定过关系{request.Relation}");
        }

        var binding = await StudentRelationBindingStore.EntityQuery
            .Where(binding => binding.UserId == userId && binding.StudentId == studentId)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (binding is not null)
        {
            binding.Relation = request.Relation;
            var updateResult = await StudentRelationBindingStore.UpdateAsync(binding, context.CancellationToken);
            return updateResult.AffectedResponse();
        }

        binding = Instance.CreateInstance<ArtemisStudentRelationBinding>();
        binding.UserId = userId;
        binding.StudentId = studentId;
        binding.Relation = request.Relation;
        var createResult = await StudentRelationBindingStore.CreateAsync(binding, context.CancellationToken);
        return createResult.AffectedResponse();
    }

    /// <summary>
    /// 上传孩子眼部照片
    /// </summary>
    /// <param name="request">The request received from the client.</param>
    /// <param name="context">The context of the server-side call handler being invoked.</param>
    /// <returns>The response to send back to the client (wrapped by a task).</returns>
    [Description("上传孩子眼部照片")]
    [Authorize(AuthorizePolicy.Token)]
    public override async Task<AffectedResponse> UploadChildEyePhoto(UploadChildEyePhotoRequest request, ServerCallContext context)
    {
        var (valid, userId) = context.GetHttpContext().GetUserId();

        if (!valid)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("解析凭据中的用户标识失败");
        }

        var userExists = await UserStore.ExistsAsync(userId, context.CancellationToken);

        if (!userExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("用户不存在");
        }

        var studentId = Guid.Parse(request.StudentId);

        var studentBindExists = await StudentRelationBindingStore
            .EntityQuery
            .Where(item => 
                item.UserId == userId && 
                item.StudentId == studentId)
            .AnyAsync(context.CancellationToken);

        if (!studentBindExists)
        {
            return ResultAdapter.AdaptEmptyFail<AffectedResponse>("孩子没有绑定用户");
        }

        var studentEysPhoto = await StudentEyePhotoStore
            .EntityQuery
            .Where(item => item.StudentId == studentId)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (studentEysPhoto is null)
        {
            studentEysPhoto = Instance.CreateInstance<ArtemisStudentEyePhoto>();
            studentEysPhoto.StudentId = studentId;

            await StudentEyePhotoStore.CreateAsync(studentEysPhoto, context.CancellationToken);
        }

        if (request.EyePhotoType == EyePhotoType.LeftEye)
        {
            studentEysPhoto.LeftEyePhoto = request.EyePhoto;
        }

        if (request.EyePhotoType == EyePhotoType.RightEye)
        {
            studentEysPhoto.RightEyePhoto = request.EyePhoto;
        }

        if (request.EyePhotoType == EyePhotoType.BothEye)
        {
            studentEysPhoto.BothEyePhoto = request.EyePhoto;
        }

        var result = await StudentEyePhotoStore.UpdateAsync(studentEysPhoto, context.CancellationToken);

        return result.AffectedResponse();
    }

    #endregion
}