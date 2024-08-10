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
    public ArtemisSchool? School { get; set; }

    /// <summary>
    ///     班主任
    /// </summary>
    public ArtemisTeacher? HeadTeacher { get; set; }

    /// <summary>
    ///     班级管理的学生
    /// </summary>
    public ICollection<ArtemisStudent>? Students { get; set; }

    /// <summary>
    /// 学生转入转出记录
    /// </summary>
    public ICollection<ArtemisStudentChangeLog>? StudentChangeLogs { get; set; }
}