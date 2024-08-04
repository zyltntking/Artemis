using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     教师实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTeacherConfiguration))]
public sealed class ArtemisTeacher : Teacher
{
    /// <summary>
    ///     教师所在的学校
    /// </summary>
    public ArtemisSchool? School { get; set; }

    /// <summary>
    ///     班主任班级
    /// </summary>
    public ArtemisClass? HeadTeacherClass { get; set; }
}