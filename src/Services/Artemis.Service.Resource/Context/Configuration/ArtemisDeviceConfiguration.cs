using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     设备实体配置
/// </summary>
internal sealed class ArtemisDeviceConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDevice>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDevice,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "设备数据集";

    /// <summary>
    ///     表明
    /// </summary>
    protected override string TableName => nameof(ArtemisDevice).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisDevice> builder)
    {
        builder.Property(entity => entity.PurchaseDate)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.InstallDate)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.WarrantyDate)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(entity => entity.MaintenanceDate)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}