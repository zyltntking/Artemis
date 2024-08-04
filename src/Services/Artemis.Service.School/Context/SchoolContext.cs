using Artemis.Service.Shared;
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

    #region Overrides of DbContext

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Project.Schemas.School);
    }

    #endregion
}