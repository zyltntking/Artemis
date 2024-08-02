using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     行政区划配置
/// </summary>
internal sealed class ArtemisDivisionConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDivision>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDivision,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "行政区划数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisDivision).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisDivision> builder)
    {
        // Each Division can have many Children Division
        builder.HasMany(division => division.Children)
            .WithOne(child => child.Parent)
            .HasForeignKey(child => child.ParentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisDivision).TableName(),
                nameof(ArtemisDivision).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}