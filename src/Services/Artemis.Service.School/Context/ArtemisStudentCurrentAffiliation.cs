using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学生当前所属关系实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStudentCurrentAffiliationConfiguration))]
public class ArtemisStudentCurrentAffiliation : StudentCurrentAffiliation
{
    /// <summary>
    ///     学校
    /// </summary>
    public ArtemisSchool? School { get; set; }

    /// <summary>
    ///     班级
    /// </summary>
    public ArtemisClass? Class { get; set; }

    /// <summary>
    ///     学生
    /// </summary>
    public required ArtemisStudent Student { get; set; }
}