using Microsoft.EntityFrameworkCore;

namespace Artemis.App.BroadcastApi.Data;

/// <summary>
///     广播数据上下文
/// </summary>
public class BroadcastContext : DbContext
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options">创建配置</param>
    public BroadcastContext(DbContextOptions<BroadcastContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///     用户模型
    /// </summary>
    public DbSet<User> Users { get; set; } = default!;

    /// <summary>
    ///     订单模型
    /// </summary>
    public DbSet<Order> Orders { get; set; } = default!;
}