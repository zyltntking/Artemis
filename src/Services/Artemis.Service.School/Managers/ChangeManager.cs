using Artemis.Data.Store;
using Artemis.Service.School.Stores;
using Microsoft.Extensions.Logging;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
using Artemis.Service.School.Context;
using Artemis.Service.Shared.School;
using Artemis.Service.Shared.School.Transfer;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Mapster;

namespace Artemis.Service.School.Managers;

/// <summary>
/// 变更管理器接口
/// </summary>
public interface IChangeManager : IManager
{
    /// <summary>
    /// 根据学校和学生信息搜索学生异动信息
    /// </summary>
    /// <param name="studentNameSearch"></param>
    /// <param name="schoolNameSearch"></param>
    /// <param name="changType"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageResult<StudentChangeLogInfo>> SearchStudentChangeLogInfoAsync(string? studentNameSearch, string? schoolNameSearch, string? changType, int page, int size, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据学校和老师信息搜索老师异动信息
    /// </summary>
    /// <param name="teacherNameSearch"></param>
    /// <param name="schoolNameSearch"></param>
    /// <param name="changeType"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageResult<TeacherChangeLogInfo>> SearchTeacherChangeLogInfoAsync(string? teacherNameSearch, string? schoolNameSearch, string? changeType, int page, int size, CancellationToken cancellationToken = default);

    /// <summary>
    /// 教师转出学校
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> SchoolTeacherMoveOutAsync(Guid teacherId, string? reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// 教师转入学校
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="schoolId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> SchoolTeacherMoveInAsync(Guid teacherId, Guid schoolId, string? reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// 学生转出学校
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> SchoolStudentMoveOutAsync(Guid studentId, string? reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// 学生转入学校
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="schoolId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> SchoolStudentMoveInAsync(Guid studentId, Guid schoolId, string? reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// 学生变更班级
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="classId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> StudentChangeClassAsync(Guid studentId, Guid classId, string? reason, CancellationToken cancellationToken = default);

}

/// <summary>
/// 变更管理器
/// </summary>
public class ChangeManager : Manager, IChangeManager
{
    /// <summary>
    /// 变更管理器实现
    /// </summary>
    /// <param name="schoolStore"></param>
    /// <param name="classStore"></param>
    /// <param name="teacherStore"></param>
    /// <param name="studentStore"></param>
    /// <param name="teacherChangeLogStore"></param>
    /// <param name="studentChangeLogStore"></param>
    /// <param name="logger"></param>
    public ChangeManager(
        IArtemisSchoolStore schoolStore, 
        IArtemisClassStore classStore, 
        IArtemisTeacherStore teacherStore, 
        IArtemisStudentStore studentStore,
        IArtemisTeacherChangeLogStore teacherChangeLogStore,
        IArtemisStudentChangeLogStore studentChangeLogStore,
        ILogger<ChangeManager>? logger = null) : base(null, logger)
    {
        SchoolStore = schoolStore;
        ClassStore = classStore;
        TeacherStore = teacherStore;
        StudentStore = studentStore;
        StudentChangeLogStore = studentChangeLogStore;
        TeacherChangeLogStore = teacherChangeLogStore;

    }

    private IArtemisSchoolStore SchoolStore { get; }

    private IArtemisClassStore ClassStore { get; }

    private IArtemisTeacherStore TeacherStore { get; }

    private IArtemisStudentStore StudentStore { get; }

    private IArtemisTeacherChangeLogStore TeacherChangeLogStore { get; }

    private IArtemisStudentChangeLogStore StudentChangeLogStore { get; }


    #region Overrides of Manager

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        SchoolStore.Dispose();
        ClassStore.Dispose();
        StudentStore.Dispose();
        StudentChangeLogStore.Dispose();
        TeacherStore.Dispose();
        StudentChangeLogStore.Dispose();
    }

    #endregion

    #region Implementation of IChangeManager

    /// <summary>
    /// 根据学校和学生信息搜索学生异动信息
    /// </summary>
    /// <param name="studentNameSearch"></param>
    /// <param name="schoolNameSearch"></param>
    /// <param name="changType"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PageResult<StudentChangeLogInfo>> SearchStudentChangeLogInfoAsync(
        string? studentNameSearch, 
        string? schoolNameSearch, 
        string? changType, 
        int page, 
        int size, 
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        studentNameSearch ??= string.Empty;
        schoolNameSearch ??= string.Empty;
        changType ??= string.Empty;

        var query = StudentChangeLogStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            studentNameSearch != string.Empty,
            student => EF.Functions.Like(
                student.StudentName, $"%{studentNameSearch}%"));

        query = query.WhereIf(
            schoolNameSearch != string.Empty,
            student => EF.Functions.Like(
                student.SchoolName, $"%{schoolNameSearch}%"));

        query = query.WhereIf(changType != string.Empty, student => student.ChangeType == changType);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderByDescending(student => student.ChangeTime);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var studentChangeLogs = await query
            .ProjectToType<StudentChangeLogInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<StudentChangeLogInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = studentChangeLogs
        };
    }

    /// <summary>
    /// 根据学校和老师信息搜索老师异动信息
    /// </summary>
    /// <param name="teacherNameSearch"></param>
    /// <param name="schoolNameSearch"></param>
    /// <param name="changType"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PageResult<TeacherChangeLogInfo>> SearchTeacherChangeLogInfoAsync(string? teacherNameSearch, string? schoolNameSearch, string? changType, int page, int size, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        OnAsyncActionExecuting(cancellationToken);

        teacherNameSearch ??= string.Empty;
        schoolNameSearch ??= string.Empty;
        changType ??= string.Empty;

        var query = TeacherChangeLogStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            teacherNameSearch != string.Empty,
            teacher => EF.Functions.Like(
                teacher.TeacherName, $"%{teacherNameSearch}%"));

        query = query.WhereIf(
            schoolNameSearch != string.Empty,
            teacher => EF.Functions.Like(
                teacher.SchoolName, $"%{schoolNameSearch}%"));

        query = query.WhereIf(changType != string.Empty, student => student.ChangeType == changType);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderByDescending(student => student.ChangeTime);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var teacherChangeLogs = await query
            .ProjectToType<TeacherChangeLogInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<TeacherChangeLogInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = teacherChangeLogs
        };
    }

    /// <summary>
    /// 教师转出学校
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> SchoolTeacherMoveOutAsync(Guid teacherId, string? reason, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var teacher = await TeacherStore.FindEntityAsync(teacherId, cancellationToken);

        if (teacher is not null)
        {
            var moveOutSchoolId = teacher.SchoolId;

            if (moveOutSchoolId is not null)
            {
                var moveOutSchool = await SchoolStore.FindEntityAsync(moveOutSchoolId.Value, cancellationToken);

                if (moveOutSchool is not null)
                {
                    var moveOutChangeLog = InitialTeacherChangeLog(teacher, moveOutSchool, ChangeType.MoveOutSchool, reason);

                    var moveOutResult = await TeacherChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);

                    teacher.SchoolId = null;

                    var teacherResult = await TeacherStore.UpdateAsync(teacher, cancellationToken);

                    return StoreResult.Success(moveOutResult.AffectRows + teacherResult.AffectRows);
                }

                return StoreResult.Failed(new StoreError
                {
                    Description = "该教师所属的学校不存在，请联系管理员"
                });
            }

            return StoreResult.Failed(new StoreError
            {
                Description = "该教师不属于任何学校，无法转出"
            });
        }

        return StoreResult.Failed(new StoreError
        {
            Description = "教师不存在，无法转出"
        });
    }

    /// <summary>
    /// 教师转入学校
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="schoolId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> SchoolTeacherMoveInAsync(Guid teacherId, Guid schoolId, string? reason, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var teacher = await TeacherStore.FindEntityAsync(teacherId, cancellationToken);

        if (teacher is not null)
        {
            var moveOutSchoolId = teacher.SchoolId;

            StoreResult moveOutResult = StoreResult.Failed();

            if (moveOutSchoolId is not null)
            {
                var moveOutSchool = await SchoolStore.FindEntityAsync(moveOutSchoolId.Value, cancellationToken);

                if (moveOutSchool is not null)
                {
                    var moveOutChangeLog = InitialTeacherChangeLog(teacher, moveOutSchool, ChangeType.MoveOutSchool, reason);

                    moveOutResult = await TeacherChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);
                }
            }

            var moveInSchool = await SchoolStore.FindEntityAsync(schoolId, cancellationToken);

            if (moveInSchool is not null)
            {
                var moveInChangeLog = InitialTeacherChangeLog(teacher, moveInSchool, ChangeType.MoveInSchool, reason);

                var moveInResult = await TeacherChangeLogStore.CreateAsync(moveInChangeLog, cancellationToken);

                teacher.SchoolId = moveInSchool.Id;

                var teacherResult = await TeacherStore.UpdateAsync(teacher, cancellationToken);

                return StoreResult.Success(moveOutResult.AffectRows + moveInResult.AffectRows + teacherResult.AffectRows);
            }

            return StoreResult.Failed(new StoreError
            {
                Description = "要转入的学校不存在，无法转入"
            });
        }

        return StoreResult.Failed(new StoreError
        {
            Description = "教师不存在，无法转出"
        });
    }

    /// <summary>
    /// 学生转出学校
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> SchoolStudentMoveOutAsync(Guid studentId, string? reason, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var student = await StudentStore.FindEntityAsync(studentId, cancellationToken);

        if (student is not null)
        {
            var moveOutSchoolId = student.SchoolId;

            if (moveOutSchoolId is not null)
            {
                var moveOutSchool = await SchoolStore.FindEntityAsync(moveOutSchoolId.Value, cancellationToken);

                if (moveOutSchool is not null)
                {
                    var moveOutChangeLog = InitialStudentChangeLog(student, moveOutSchool, null, ChangeType.MoveInSchool, reason);

                    var moveOutResult = await StudentChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);

                    student.SchoolId = null;
                    student.ClassId = null;

                    var studentResult = await StudentStore.UpdateAsync(student, cancellationToken);

                    return StoreResult.Success(moveOutResult.AffectRows + studentResult.AffectRows);
                }

                return StoreResult.Failed(new StoreError
                {
                    Description = "该学生所属的学校不存在，请联系管理员"
                });
            }

            return StoreResult.Failed(new StoreError
            {
                Description = "该学生不属于任何学校，无法转出"
            });
        }

        return StoreResult.Failed(new StoreError
        {
            Description = "学生不存在，无法转出"
        });
    }

    /// <summary>
    /// 学生转入学校
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="schoolId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> SchoolStudentMoveInAsync(Guid studentId, Guid schoolId, string? reason, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var student = await StudentStore.FindEntityAsync(studentId, cancellationToken);

        if (student is not null)
        {
            var moveOutSchoolId = student.SchoolId;

            StoreResult moveOutResult = StoreResult.Failed();

            if (moveOutSchoolId is not null)
            {
                var moveOutSchool = await SchoolStore.FindEntityAsync(moveOutSchoolId.Value, cancellationToken);

                if (moveOutSchool is not null)
                {
                    var moveOutChangeLog = InitialStudentChangeLog(student, moveOutSchool, null, ChangeType.MoveInSchool, reason);

                    moveOutResult = await StudentChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);
                }
            }

            var moveInSchool = await SchoolStore.FindEntityAsync(schoolId, cancellationToken);

            if (moveInSchool is not null)
            {
                var moveInChangeLog = InitialStudentChangeLog(student, moveInSchool, null, ChangeType.MoveInSchool, reason);

                var moveInResult = await StudentChangeLogStore.CreateAsync(moveInChangeLog, cancellationToken);

                student.SchoolId = moveInSchool.Id;
                student.ClassId = null;

                var studentResult = await StudentStore.UpdateAsync(student, cancellationToken);

                return StoreResult.Success(moveOutResult.AffectRows + moveInResult.AffectRows + studentResult.AffectRows);
            }

            return StoreResult.Failed(new StoreError
            {
                Description = "要转入的学校不存在，无法转入"
            });
        }

        return StoreResult.Failed(new StoreError
        {
            Description = "学生不存在，无法转出"
        });
    }

    /// <summary>
    /// 学生变更班级
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="classId"></param>
    /// <param name="reason"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<StoreResult> StudentChangeClassAsync(Guid studentId, Guid classId, string? reason, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        var student = await StudentStore.FindEntityAsync(studentId, cancellationToken);

        if (student is not null)
        {
            var moveInClass = await ClassStore.FindEntityAsync(classId, cancellationToken);

            if (moveInClass is not null)
            {
                if (moveInClass.SchoolId != student.SchoolId)
                {
                    return StoreResult.Failed(new StoreError
                    {
                        Description = "要转入的班级属于其他学校，无法变更"
                    });
                }

                if (moveInClass.Id == student.ClassId)
                {
                    return StoreResult.Failed(new StoreError
                    {
                        Description = "班级未变更"
                    });
                }

                var school = await SchoolStore.FindEntityAsync(moveInClass.SchoolId, cancellationToken);

                if (school == null)
                {
                    return StoreResult.Failed(new StoreError
                    {
                        Description = "要转入的班级所属的学校不存在，无法转入"
                    });
                }

                var moveOutResult = StoreResult.Failed();

                if (student.ClassId != null)
                {
                    var moveOutClass = await ClassStore.FindEntityAsync(student.ClassId.Value, cancellationToken);

                    if (moveOutClass is not null)
                    {
                        var moveOutChangeLog = InitialStudentChangeLog(student, school, moveOutClass, ChangeType.MoveOutClass, reason);

                        moveOutResult = await StudentChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);
                    }
                }

                var moveInChangeLog = InitialStudentChangeLog(student, school, moveInClass, ChangeType.MoveInClass, reason);

                var moveInResult = await StudentChangeLogStore.CreateAsync(moveInChangeLog, cancellationToken);

                student.SchoolId = school.Id;
                student.ClassId = moveInClass.Id;

                var studentResult = await StudentStore.UpdateAsync(student, cancellationToken);

                return StoreResult.Success(moveOutResult.AffectRows + moveInResult.AffectRows + studentResult.AffectRows);

            }

            return StoreResult.Failed(new StoreError
            {
                Description = "要变更的班级不存在，无法变更"
            });
        }

        return StoreResult.Failed(new StoreError
        {
            Description = "学生不存在，无法转出"
        });
    }

    #endregion

    /// <summary>
    /// 初始化学生变动记录
    /// </summary>
    /// <param name="studentInfo"></param>
    /// <param name="schoolInfo"></param>
    /// <param name="classInfo"></param>
    /// <param name="type"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    private ArtemisStudentChangeLog InitialStudentChangeLog(
        IStudent studentInfo, 
        ISchool schoolInfo, 
        IClass? classInfo, 
        ChangeType type, 
        string? reason)
    {
        var changeLog = Instance.CreateInstance<ArtemisStudentChangeLog>();
        changeLog.SchoolId = schoolInfo.Id;
        changeLog.SchoolName = schoolInfo.Name;
        changeLog.DivisionCode = schoolInfo.DivisionCode;
        changeLog.ClassId = classInfo?.Id;
        changeLog.ClassName = classInfo?.Name;
        changeLog.GradeName = classInfo?.GradeName;
        changeLog.SerialNumber = classInfo?.SerialNumber;
        changeLog.StudentId = studentInfo.Id;
        changeLog.SchoolName = studentInfo.Name;
        changeLog.Birthday = studentInfo.Birthday;
        changeLog.ChangeType = type;
        changeLog.ChangeTime = DateTime.Now;
        changeLog.ChangeReason = reason;

        return changeLog;
    }

    /// <summary>
    /// 初始化教师变动记录
    /// </summary>
    /// <param name="teacherInfo"></param>
    /// <param name="schoolInfo"></param>
    /// <param name="type"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    private ArtemisTeacherChangeLog InitialTeacherChangeLog(ITeacher teacherInfo, ISchool schoolInfo, ChangeType type, string? reason)
    {
        var changeLog = Instance.CreateInstance<ArtemisTeacherChangeLog>();

        changeLog.SchoolId = schoolInfo.Id;
        changeLog.SchoolName = schoolInfo.Name;
        changeLog.DivisionCode = schoolInfo.DivisionCode;
        changeLog.TeacherId = teacherInfo.Id;
        changeLog.TeacherName = teacherInfo.Name;
        changeLog.ChangeType = type;
        changeLog.ChangeTime = DateTime.Now;
        changeLog.ChangeReason = reason;

        return changeLog;
    }
}