﻿using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
///     原始数据上下文
/// </summary>
public class BusinessContext : DbContext
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="options">配置</param>
    public BusinessContext(DbContextOptions<BusinessContext> options) : base(options)
    {
    }

    /// <summary>
    ///     验光仪数据
    /// </summary>
    public virtual DbSet<ArtemisOptometer> Optometers { get; set; } = default!;

    /// <summary>
    ///     视力表数据
    /// </summary>
    public virtual DbSet<ArtemisVisualChart> VisualCharts { get; set; } = default!;

    #region Overrides of DbContext

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Project.Schemas.Business);
    }

    #endregion
}