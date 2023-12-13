using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.App.BroadcastApi.Data.Configuration;

/// <summary>
///     订单配置
/// </summary>
public class OrderConfiguration : BroadcastConfiguration<Order>
{
    #region Overrides of BroadcastConfiguration<Order>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "订单数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(Order);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<Order> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(order => order.Id)
            .HasComment("标识");

        builder.Property(order => order.License)
            .HasMaxLength(10)
            .IsRequired()
            .HasComment("车牌号");

        builder.Property(order => order.Count)
            .IsRequired()
            .HasComment("用餐人数");

        builder.Property(order => order.Price)
            .IsRequired()
            .HasComment("餐价");

        builder.Property(order => order.MealDate)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("用餐日期");

        builder.Property(order => order.MealType)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("用餐类型");

        builder.Property(order => order.Status)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("状态");

        builder.Property(order => order.WaitFlag)
            .IsRequired()
            .HasComment("等待序列");

        builder.Property(order => order.Remark)
            .HasMaxLength(256)
            .HasComment("备注");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<Order> builder)
    {
        MetaIndexConfigure(builder);

        // User Key
        builder.HasKey(order => order.Id)
            .HasName($"PK_{TableName}");

        // User Name Index
        builder.HasIndex(order => order.Status)
            .HasDatabaseName($"IX_{TableName}_Status");

        builder.HasIndex(order => order.MealDate)
            .HasDatabaseName($"IX_{TableName}_MealDate");

        builder.HasIndex(order => order.WaitFlag)
            .HasDatabaseName($"IX_{TableName}_WaitFlag");
    }

    #endregion
}