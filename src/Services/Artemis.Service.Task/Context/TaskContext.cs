using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
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
        modelBuilder.HasDefaultSchema(Project.Schemas.Task);

        // Task Unit Agent Map
        modelBuilder.Entity<ArtemisTaskUnit>()
            .HasMany(taskUnit => taskUnit.Agents) //left side
            .WithMany(agent => agent.TaskUnits) //right side
            .UsingEntity<ArtemisTaskAgent>(
                left => left
                    .HasOne(taskAgent => taskAgent.Agent)
                    .WithMany(agent => agent.TaskAgents)
                    .HasForeignKey(taskAgent => taskAgent.AgentId)
                    .HasConstraintName(nameof(ArtemisTaskAgent).TableName()
                        .ForeignKeyName(nameof(ArtemisAgent).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(agent => agent.Id),
                right => right
                    .HasOne(taskAgent => taskAgent.TaskUnit)
                    .WithMany(unit => unit.TaskAgents)
                    .HasForeignKey(taskAgent => taskAgent.TaskUnitId)
                    .HasConstraintName(nameof(ArtemisTaskAgent).TableName()
                        .ForeignKeyName(nameof(ArtemisTaskUnit).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(unit => unit.Id),
                taskAgentJoin =>
                {
                    taskAgentJoin.HasKey(taskAgent => new { taskAgent.TaskUnitId, taskAgent.AgentId })
                        .HasName(nameof(ArtemisTaskAgent).TableName().KeyName());
                });

        // Task Agent Map
        modelBuilder.Entity<ArtemisTask>()
            .HasMany(task => task.Agents) //left side
            .WithMany(agent => agent.Tasks) //right side
            .UsingEntity<ArtemisTaskAgent>(
                left => left
                    .HasOne(taskAgent => taskAgent.Agent)
                    .WithMany(agent => agent.TaskAgents)
                    .HasForeignKey(taskAgent => taskAgent.AgentId)
                    .HasConstraintName(nameof(ArtemisTaskAgent).TableName()
                        .ForeignKeyName(nameof(ArtemisAgent).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(agent => agent.Id),
                right => right
                    .HasOne(taskAgent => taskAgent.Task)
                    .WithMany(task => task.TaskAgents)
                    .HasForeignKey(taskAgent => taskAgent.TaskId)
                    .HasConstraintName(nameof(ArtemisTaskAgent).TableName()
                        .ForeignKeyName(nameof(ArtemisTask).TableName()))
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasPrincipalKey(unit => unit.Id));
    }
}