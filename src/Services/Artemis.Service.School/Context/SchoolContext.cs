using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context;

/// <summary>
///     学校业务上下文
/// </summary>
public class SchoolContext : DbContext
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="options">配置</param>
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }

    /// <summary>
    ///     学校数据集
    /// </summary>
    public virtual DbSet<ArtemisSchool> Schools { get; set; } = default!;

    /// <summary>
    ///     班级数据集
    /// </summary>
    public virtual DbSet<ArtemisClass> Classes { get; set; } = default!;

    /// <summary>
    ///     学生数据集
    /// </summary>
    public virtual DbSet<ArtemisStudent> Students { get; set; } = default!;

    /// <summary>
    ///     教师数据集
    /// </summary>
    public virtual DbSet<ArtemisTeacher> Teachers { get; set; } = default!;

    /// <summary>
    ///     班级学生数对应据集
    /// </summary>
    public virtual DbSet<ArtemisClassStudent> ClassStudents { get; set; } = default!;

    /// <summary>
    ///     班级教师数据集
    /// </summary>
    public virtual DbSet<ArtemisClassTeacher> ClassTeachers { get; set; } = default!;

    /// <summary>
    ///     学校教师数据集
    /// </summary>
    public virtual DbSet<ArtemisSchoolTeacher> SchoolTeachers { get; set; } = default!;

    /// <summary>
    ///     学校学生数据集
    /// </summary>
    public virtual DbSet<ArtemisSchoolStudent> SchoolStudents { get; set; } = default!;

    /// <summary>
    ///     教师当前所属关系数据集
    /// </summary>
    public virtual DbSet<ArtemisTeacherCurrentAffiliation> TeacherCurrentAffiliations { get; set; } = default!;

    /// <summary>
    ///     学生当前所属关系数据集
    /// </summary>
    public virtual DbSet<ArtemisStudentCurrentAffiliation> StudentCurrentAffiliations { get; set; } = default!;

    #region Overrides of DbContext

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("school");

        //Class Student Map
        modelBuilder.Entity<ArtemisClass>()
            .HasMany(iClass => iClass.Students)
            .WithMany(student => student.Classes)
            .UsingEntity<ArtemisClassStudent>(
                left => left
                    .HasOne(classStudent => classStudent.Student)
                    .WithMany(student => student.ClassStudents)
                    .HasForeignKey(classStudent => classStudent.StudentId)
                    .HasConstraintName(nameof(ArtemisClassStudent).TableName()
                        .ForeignKeyName(nameof(ArtemisStudent).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(student => student.Id),
                right => right
                    .HasOne(classStudent => classStudent.Class)
                    .WithMany(iClass => iClass.ClassStudents)
                    .HasForeignKey(classStudent => classStudent.ClassId)
                    .HasConstraintName(nameof(ArtemisClassStudent).TableName()
                        .ForeignKeyName(nameof(ArtemisClass).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(iClass => iClass.Id),
                classStudentJoin =>
                {   
                    classStudentJoin.HasKey(classStudent => new { classStudent.ClassId, classStudent.StudentId, classStudent.MoveIn })
                        .HasName(nameof(ArtemisClassStudent).TableName().KeyName());
                }
            );

        //Class Teacher Map
        modelBuilder.Entity<ArtemisClass>()
            .HasMany(iClass => iClass.Teachers)
            .WithMany(teacher => teacher.Classes)
            .UsingEntity<ArtemisClassTeacher>(
                left => left
                    .HasOne(classTeacher => classTeacher.Teacher)
                    .WithMany(teacher => teacher.ClassTeachers)
                    .HasForeignKey(classTeacher => classTeacher.TeacherId)
                    .HasConstraintName(nameof(ArtemisClassTeacher).TableName()
                        .ForeignKeyName(nameof(ArtemisTeacher).TableName()))    
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(teacher => teacher.Id),
                right => right
                    .HasOne(classTeacher => classTeacher.Class)
                    .WithMany(iClass => iClass.ClassTeachers)
                    .HasForeignKey(classTeacher => classTeacher.ClassId)
                    .HasConstraintName(nameof(ArtemisClassTeacher).TableName()
                        .ForeignKeyName(nameof(ArtemisClass).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(iClass => iClass.Id),
                classTeacherJoin =>
                {
                    classTeacherJoin.HasKey(classTeacher => new { classTeacher.ClassId, classTeacher.TeacherId, classTeacher.MoveIn })
                        .HasName(nameof(ArtemisClassTeacher).TableName().KeyName());
                });

        //School Student Map
        modelBuilder.Entity<ArtemisSchool>()
            .HasMany(school => school.Students)
            .WithMany(student => student.Schools)
            .UsingEntity<ArtemisSchoolStudent>(
                left => left
                    .HasOne(schoolStudent => schoolStudent.Student)
                    .WithMany(student => student.SchoolStudents)
                    .HasForeignKey(schoolStudent => schoolStudent.StudentId)
                    .HasConstraintName(nameof(ArtemisSchoolStudent).TableName()
                        .ForeignKeyName(nameof(ArtemisStudent).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(student => student.Id),
                right => right
                    .HasOne(schoolStudent => schoolStudent.School)
                    .WithMany(school => school.SchoolStudents)
                    .HasForeignKey(schoolStudent => schoolStudent.SchoolId)
                    .HasConstraintName(nameof(ArtemisSchoolStudent).TableName()
                        .ForeignKeyName(nameof(ArtemisSchool).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(school => school.Id),
                schoolStudentJoin =>
                {
                    schoolStudentJoin.HasKey(schoolStudent => new { schoolStudent.SchoolId, schoolStudent.StudentId, schoolStudent.MoveIn })
                        .HasName(nameof(ArtemisSchoolStudent).TableName().KeyName());
                });

        //School Teacher Map
        modelBuilder.Entity<ArtemisSchool>()
            .HasMany(school => school.Teachers)
            .WithMany(teacher => teacher.Schools)
            .UsingEntity<ArtemisSchoolTeacher>(
                left => left
                    .HasOne(schoolTeacher => schoolTeacher.Teacher)
                    .WithMany(teacher => teacher.SchoolTeachers)
                    .HasForeignKey(schoolTeacher => schoolTeacher.TeacherId)
                    .HasConstraintName(nameof(ArtemisSchoolTeacher).TableName()
                        .ForeignKeyName(nameof(ArtemisTeacher).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(teacher => teacher.Id),
                right => right  
                    .HasOne(schoolTeacher => schoolTeacher.School)
                    .WithMany(school => school.SchoolTeachers)
                    .HasForeignKey(schoolTeacher => schoolTeacher.SchoolId)
                    .HasConstraintName(nameof(ArtemisSchoolTeacher).TableName()
                        .ForeignKeyName(nameof(ArtemisSchool).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(school => school.Id),
                schoolTeacherJoin =>
                {
                    schoolTeacherJoin.HasKey(schoolTeacher => new { schoolTeacher.SchoolId, schoolTeacher.TeacherId, schoolTeacher.MoveIn })
                        .HasName(nameof(ArtemisSchoolTeacher).TableName().KeyName());
                });
    }

    #endregion
}