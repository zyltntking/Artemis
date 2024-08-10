using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
/// 教师变更记录实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTeacherChangeLogConfiguration))]
public class ArtemisTeacherChangeLog : TeacherChangeLog
{
    /// <summary>
    /// 变更关联的学生
    /// </summary>
    public ArtemisTeacher? Teacher { get; set; }

    /// <summary>
    /// 变更关联的学校
    /// </summary>
    public ArtemisSchool? School { get; set; }
}