using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Task.Context;

/// <summary>
///     任务业务上下文
/// </summary>
public class TaskContext : DbContext
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="options">配置</param>
    public TaskContext(DbContextOptions<TaskContext> options) : base(options)
    {
    }

    /// <summary>
    ///     任务数据集
    /// </summary>
    public virtual DbSet<ArtemisTask> Tasks { get; set; }

    /// <summary>
    ///     代理数据集
    /// </summary>
    public virtual DbSet<ArtemisAgent> Agents { get; set; }

    /// <summary>
    ///     任务单元数据集
    /// </summary>
    public virtual DbSet<ArtemisTaskUnit> TaskUnits { get; set; }

    /// <summary>
    ///     任务目标数据集
    /// </summary>
    public virtual DbSet<ArtemisTaskTarget> TaskTargets { get; set; }

    /// <summary>
    ///     任务代理数据集
    /// </summary>
    public virtual DbSet<ArtemisTaskAgent> TaskAgents { get; set; }

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("task");
    }
}