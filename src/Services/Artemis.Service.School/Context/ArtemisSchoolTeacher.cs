using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学校教师关系对应实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisSchoolTeacherConfiguration))]
public sealed class ArtemisSchoolTeacher : SchoolTeacher
{
    /// <summary>
    ///     对应学校
    /// </summary>
    public required ArtemisSchool School { get; set; }

    /// <summary>
    ///     对应教师
    /// </summary>
    public required ArtemisTeacher Teacher { get; set; }
}