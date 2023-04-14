using Artemis.Data.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store;

/// <summary>
/// 存储模型
/// </summary>
public class StoreModel : PartitionBase
{
    /// <summary>
    ///     标识
    /// </summary>
    [Comment("标识")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     分区标识
    /// </summary>
    [Comment("分区标识")]
    public override int Partition { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    [Comment("创建时间，生成以后不再进行改动")]
    public override DateTime CreatedAt { get; set; }

    /// <summary>
    ///     更新时间
    /// </summary>
    [Comment("更新时间，初始化为生成时间")]
    public override DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     删除时间
    /// </summary>
    [Comment("删除时间，启用软删除以后生效，标识数据已被删除")]
    public override DateTime? DeletedAt { get; set; }
}