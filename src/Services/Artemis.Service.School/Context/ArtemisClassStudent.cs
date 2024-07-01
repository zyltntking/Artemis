using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     班级学生关系对应实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisClassStudentConfiguration))]
public sealed class ArtemisClassStudent : ClassStudent
{
    /// <summary>
    ///     关联班级
    /// </summary>
    public required ArtemisClass Class { get; set; }

    /// <summary>
    ///     关联学生
    /// </summary>
    public required ArtemisStudent Student { get; set; }
}