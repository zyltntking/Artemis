using Artemis.Data.Store;
using Artemis.Service.School.Context;
using Artemis.Service.School.Stores;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Managers;

/// <summary>
///     学校管理器
/// </summary>
public class ArtemisSchoolManager : Manager<ArtemisSchool, Guid, Guid>, IArtemisSchoolManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="schoolStore">存储访问器依赖</param>
    /// <param name="teacherStudentStore"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="classStore"></param>
    /// <param name="teacherStore"></param>
    /// <param name="studentStore"></param>
    /// <param name="schoolTeacherStore"></param>
    /// <param name="schoolStudentStore"></param>
    /// <param name="classTeacherStore"></param>
    /// <param name="classStudentStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisSchoolManager(
        IArtemisSchoolStore schoolStore,
        IArtemisClassStore classStore,
        IArtemisTeacherStore teacherStore,
        IArtemisStudentStore studentStore,
        IArtemisSchoolTeacherStore schoolTeacherStore,
        IArtemisSchoolStudentStore schoolStudentStore,
        IArtemisClassTeacherStore classTeacherStore,
        IArtemisClassStudentStore classStudentStore,
        IArtemisTeacherStudentStore teacherStudentStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(schoolStore, options, logger)
    {
        SchoolStore = schoolStore ?? throw new ArgumentNullException(nameof(schoolStore));
        ClassStore = classStore ?? throw new ArgumentNullException(nameof(classStore));
        TeacherStore = teacherStore ?? throw new ArgumentNullException(nameof(teacherStore));
        StudentStore = studentStore ?? throw new ArgumentNullException(nameof(studentStore));
        SchoolTeacherStore = schoolTeacherStore ?? throw new ArgumentNullException(nameof(schoolTeacherStore));
        SchoolStudentStore = schoolStudentStore ?? throw new ArgumentNullException(nameof(schoolStudentStore));
        ClassTeacherStore = classTeacherStore ?? throw new ArgumentNullException(nameof(classTeacherStore));
        ClassStudentStore = classStudentStore ?? throw new ArgumentNullException(nameof(classStudentStore));
        TeacherStudentStore = teacherStudentStore ?? throw new ArgumentNullException(nameof(teacherStudentStore));
    }

    #region Overrides of KeyLessManager<ArtemisSchool,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        SchoolStore.Dispose();
        ClassStore.Dispose();
        TeacherStore.Dispose();
        StudentStore.Dispose();
        SchoolTeacherStore.Dispose();
        SchoolStudentStore.Dispose();
        ClassTeacherStore.Dispose();
        ClassStudentStore.Dispose();
        TeacherStudentStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     学校存储访问器
    /// </summary>
    private IArtemisSchoolStore SchoolStore { get; }

    /// <summary>
    ///     班级存储访问器
    /// </summary>
    private IArtemisClassStore ClassStore { get; }

    /// <summary>
    ///     教师存储访问器
    /// </summary>
    private IArtemisTeacherStore TeacherStore { get; }

    /// <summary>
    ///     学生存储访问器
    /// </summary>
    private IArtemisStudentStore StudentStore { get; }

    /// <summary>
    ///     学校教师关系存储访问器
    /// </summary>
    private IArtemisSchoolTeacherStore SchoolTeacherStore { get; }

    /// <summary>
    ///     学校学生关系存储访问器
    /// </summary>
    private IArtemisSchoolStudentStore SchoolStudentStore { get; }

    /// <summary>
    ///     班级教师关系存储访问器
    /// </summary>
    private IArtemisClassTeacherStore ClassTeacherStore { get; }

    /// <summary>
    ///     班级学生关系存储访问器
    /// </summary>
    private IArtemisClassStudentStore ClassStudentStore { get; }

    /// <summary>
    ///     教师学生关系存储访问器
    /// </summary>
    private IArtemisTeacherStudentStore TeacherStudentStore { get; }

    #endregion
}