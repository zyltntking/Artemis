using Artemis.Data.Store;
using Artemis.Service.School.Stores;
using Microsoft.Extensions.Logging;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
using Artemis.Service.School.Context;

namespace Artemis.Service.School.Managers;

/// <summary>
/// 变更管理器接口
/// </summary>
public interface IChangeManager : IManager
{
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
                    var moveOutChangeLog = Instance.CreateInstance<ArtemisTeacherChangeLog>();

                    moveOutChangeLog.SchoolId = moveOutSchool.Id;
                    moveOutChangeLog.SchoolName = moveOutSchool.Name;
                    moveOutChangeLog.TeacherId = teacher.Id;
                    moveOutChangeLog.TeacherName = teacher.Name;
                    moveOutChangeLog.ChangeType = ChangeType.MoveOutSchool;
                    moveOutChangeLog.ChangeTime = DateTime.Now;
                    moveOutChangeLog.ChangeReason = reason;

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
                    var moveOutChangeLog = Instance.CreateInstance<ArtemisTeacherChangeLog>();

                    moveOutChangeLog.SchoolId = moveOutSchool.Id;
                    moveOutChangeLog.SchoolName = moveOutSchool.Name;
                    moveOutChangeLog.TeacherId = teacher.Id;
                    moveOutChangeLog.TeacherName = teacher.Name;
                    moveOutChangeLog.ChangeType = ChangeType.MoveOutSchool;
                    moveOutChangeLog.ChangeTime = DateTime.Now;
                    moveOutChangeLog.ChangeReason = reason;

                    moveOutResult = await TeacherChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);
                }
            }

            var moveInSchool = await SchoolStore.FindEntityAsync(schoolId, cancellationToken);

            if (moveInSchool is not null)
            {
                var moveInChangeLog = Instance.CreateInstance<ArtemisTeacherChangeLog>();

                moveInChangeLog.SchoolId = moveInSchool.Id;
                moveInChangeLog.SchoolName = moveInSchool.Name;
                moveInChangeLog.TeacherId = teacher.Id;
                moveInChangeLog.TeacherName = teacher.Name;
                moveInChangeLog.ChangeType = ChangeType.MoveInSchool;
                moveInChangeLog.ChangeTime = DateTime.Now;
                moveInChangeLog.ChangeReason = reason;

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
                    var moveOutChangeLog = Instance.CreateInstance<ArtemisStudentChangeLog>();

                    moveOutChangeLog.SchoolId = moveOutSchool.Id;
                    moveOutChangeLog.SchoolName = moveOutSchool.Name;
                    moveOutChangeLog.StudentId = student.Id;
                    moveOutChangeLog.StudentName = student.Name;
                    moveOutChangeLog.ChangeType = ChangeType.MoveOutSchool;
                    moveOutChangeLog.ChangeTime = DateTime.Now;
                    moveOutChangeLog.ChangeReason = reason;

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
                    var moveOutChangeLog = Instance.CreateInstance<ArtemisStudentChangeLog>();

                    moveOutChangeLog.SchoolId = moveOutSchool.Id;
                    moveOutChangeLog.SchoolName = moveOutSchool.Name;
                    moveOutChangeLog.StudentId = student.Id;
                    moveOutChangeLog.StudentName = student.Name;
                    moveOutChangeLog.ChangeType = ChangeType.MoveOutSchool;
                    moveOutChangeLog.ChangeTime = DateTime.Now;
                    moveOutChangeLog.ChangeReason = reason;

                    moveOutResult = await StudentChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);
                }
            }

            var moveInSchool = await SchoolStore.FindEntityAsync(schoolId, cancellationToken);

            if (moveInSchool is not null)
            {
                var moveInChangeLog = Instance.CreateInstance<ArtemisStudentChangeLog>();

                moveInChangeLog.SchoolId = moveInSchool.Id;
                moveInChangeLog.SchoolName = moveInSchool.Name;
                moveInChangeLog.StudentId = student.Id;
                moveInChangeLog.SchoolName = student.Name;
                moveInChangeLog.ChangeType = ChangeType.MoveInSchool;
                moveInChangeLog.ChangeTime = DateTime.Now;
                moveInChangeLog.ChangeReason = reason;

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

                if (moveInClass.Id == classId)
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

                StoreResult moveOutResult = StoreResult.Failed();

                if (student.ClassId != null)
                {
                    var moveOutClass = await ClassStore.FindEntityAsync(student.ClassId.Value, cancellationToken);

                    if (moveOutClass is not null)
                    {
                        var moveOutChangeLog = Instance.CreateInstance<ArtemisStudentChangeLog>();
                        moveOutChangeLog.SchoolId = school.Id;
                        moveOutChangeLog.SchoolName = school.Name;
                        moveOutChangeLog.ClassId = moveOutClass.Id;
                        moveOutChangeLog.ClassName = moveOutClass.Name;
                        moveOutChangeLog.StudentId = student.Id;
                        moveOutChangeLog.StudentName = student.Name;
                        moveOutChangeLog.ChangeType = ChangeType.MoveOutClass;
                        moveOutChangeLog.ChangeTime = DateTime.Now;
                        moveOutChangeLog.ChangeReason = reason;

                        moveOutResult = await StudentChangeLogStore.CreateAsync(moveOutChangeLog, cancellationToken);
                    }
                }

                var moveInChangeLog = Instance.CreateInstance<ArtemisStudentChangeLog>();
                moveInChangeLog.SchoolId = school.Id;
                moveInChangeLog.SchoolName = school.Name;
                moveInChangeLog.ClassId = moveInClass.Id;
                moveInChangeLog.ClassName = moveInClass.Name;
                moveInChangeLog.StudentId = student.Id;
                moveInChangeLog.SchoolName = student.Name;
                moveInChangeLog.ChangeType = ChangeType.MoveInSchool;
                moveInChangeLog.ChangeTime = DateTime.Now;
                moveInChangeLog.ChangeReason = reason;

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
}