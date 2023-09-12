using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Data.Store.Configuration;

/// <summary>
///     模型存储配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class ArtemisModelConfiguration<TEntity> :ArtemisMateSlotConfiguration<TEntity> where TEntity : class, IModelBase
{
    #region Overrides of ArtemisMateSlotConfiguration<TEntity>

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<TEntity> builder)
    {
        base.FieldConfigure(builder);
        builder.Property(entity => entity.Id).HasComment("标识");
    }

    #endregion

}