using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
/// 学生变更记录实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStudentChangeLogConfiguration))]
public sealed class ArtemisStudentChangeLog : StudentChangeLog
{
    /// <summary>
    /// 变更关联的学生
    /// </summary>
    public ArtemisStudent Student { get; set; }

    /// <summary>
    /// 变更关联的学校
    /// </summary>
    public ArtemisSchool School { get; set; }

    /// <summary>
    /// 变更关联的班级
    /// </summary>
    public ArtemisClass? Class { get; set; }
}