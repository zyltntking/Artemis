using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     教师学生关系对应实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTeacherStudentConfiguration))]
public sealed class ArtemisTeacherStudent : TeacherStudent
{
    /// <summary>
    ///     对应教师
    /// </summary>
    public required ArtemisTeacher Teacher { get; set; }

    /// <summary>
    ///     对应学生
    /// </summary>
    public required ArtemisStudent Student { get; set; }
}