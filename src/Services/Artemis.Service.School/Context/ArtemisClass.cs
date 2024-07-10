using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     班级实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisClassConfiguration))]
public sealed class ArtemisClass : Class
{
    /// <summary>
    ///     班级所属的学校
    /// </summary>
    public required ArtemisSchool School { get; set; }

    /// <summary>
    /// 班主任
    /// </summary>
    public required ArtemisTeacher HeadTeacher { get; set; }

    /// <summary>
    ///     班级学生对应关系
    /// </summary>
    public ICollection<ArtemisClassStudent>? ClassStudents { get; set; }

    /// <summary>
    ///     班级管理的学生
    /// </summary>
    public ICollection<ArtemisStudent>? Students { get; set; }

    /// <summary>
    ///     学生当前所属关系
    /// </summary>
    public ICollection<ArtemisStudentCurrentAffiliation>? CurrentStudents { get; set; }

    /// <summary>
    ///     班级老师对应关系
    /// </summary>
    public ICollection<ArtemisClassTeacher>? ClassTeachers { get; set; }

    /// <summary>
    ///     班级所属的教师
    /// </summary>
    public ICollection<ArtemisTeacher>? Teachers { get; set; }

    /// <summary>
    ///     教师当前所属关系
    /// </summary>
    public ICollection<ArtemisTeacherCurrentAffiliation>? CurrentTeachers { get; set; }
}