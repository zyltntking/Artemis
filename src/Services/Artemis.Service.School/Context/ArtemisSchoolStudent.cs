using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学校学生关系对应实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisSchoolStudentConfiguration))]
public sealed class ArtemisSchoolStudent : SchoolStudent
{
    /// <summary>
    ///     对应学校
    /// </summary>
    public required ArtemisSchool School { get; set; }

    /// <summary>
    ///     对应学生
    /// </summary>
    public required ArtemisStudent Student { get; set; }
}