using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学生实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStudentConfiguration))]
public sealed class ArtemisStudent : Student
{
    /// <summary>
    ///     学生所在的学校
    /// </summary>
    public ArtemisSchool? School { get; set; }

    /// <summary>
    ///     学生所在的班级
    /// </summary>
    public ArtemisClass? Class { get; set; }
}