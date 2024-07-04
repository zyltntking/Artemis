using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     教师当前所属关系实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTeacherCurrentAffiliationConfiguration))]
public sealed class ArtemisTeacherCurrentAffiliation : TeacherCurrentAffiliation
{
    /// <summary>
    ///     教师
    /// </summary>
    public required ArtemisTeacher Teacher { get; set; }

    /// <summary>
    ///     学校
    /// </summary>
    public ArtemisSchool? School { get; set; }

    /// <summary>
    ///     班级
    /// </summary>
    public ArtemisClass? Class { get; set; }
}