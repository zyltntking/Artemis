using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     班级教师关系对应实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisClassTeacherConfiguration))]
public sealed class ArtemisClassTeacher : ClassTeacher
{
    /// <summary>
    ///     关联班级
    /// </summary>
    public required ArtemisClass Class { get; set; }

    /// <summary>
    ///     关联教师
    /// </summary>
    public required ArtemisTeacher Teacher { get; set; }
}