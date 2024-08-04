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
    public required ArtemisSchool School { get; set; }

    /// <summary>
    ///     班主任
    /// </summary>
    public required ArtemisTeacher HeadTeacher { get; set; }

    /// <summary>
    ///     班级管理的学生
    /// </summary>
    public ICollection<ArtemisStudent>? Students { get; set; }
}