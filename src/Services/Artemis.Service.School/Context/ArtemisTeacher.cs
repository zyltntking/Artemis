﻿using Artemis.Service.School.Context.Configuration;
using Artemis.Service.School.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     教师实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTeacherConfiguration))]
public sealed class ArtemisTeacher : Teacher
{
    /// <summary>
    ///     学校教师对应关系
    /// </summary>
    public ICollection<ArtemisSchoolTeacher>? SchoolTeachers { get; set; }

    /// <summary>
    ///     教师所在的学校
    /// </summary>
    public ICollection<ArtemisSchool>? Schools { get; set; }

    /// <summary>
    ///     班级老师对应关系
    /// </summary>
    public ICollection<ArtemisClassTeacher>? ClassTeachers { get; set; }

    /// <summary>
    ///     教师所在的班级
    /// </summary>
    public ICollection<ArtemisClass>? Classes { get; set; }

    /// <summary>
    ///     班主任班级
    /// </summary>
    public ArtemisClass? HeadTeacherClass { get; set; }
}