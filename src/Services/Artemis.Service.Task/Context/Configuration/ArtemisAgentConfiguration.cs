using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Task.Context.Configuration;

/// <summary>
///     代理数据集配置
/// </summary>
internal sealed class ArtemisAgentConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisAgent>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisAgent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "代理数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisAgent);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisAgent> builder)
    {
        // Each Agent can have many Task Agents
        builder.HasMany(agent => agent.TaskAgents)
            .WithOne(taskAgent => taskAgent.Agent)
            .HasForeignKey(taskAgent => taskAgent.AgentId)
            .HasConstraintName(ForeignKeyName(nameof(ArtemisTaskAgent), nameof(ArtemisAgent)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}