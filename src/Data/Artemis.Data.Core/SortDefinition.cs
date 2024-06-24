using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Core;

#region interface

/// <summary>
///     排序插件
/// </summary>
file interface ISortSlot
{
    /// <summary>
    ///     排序字段
    /// </summary>
    string Field { get; set; }

    /// <summary>
    ///     是否倒序
    /// </summary>
    OrderType Order { get; set; }
}

/// <summary>
///     排序定义
/// </summary>
file interface ISortDefinition : ISortSlot
{
    /// <summary>
    ///     顺序索引
    /// </summary>
    int Index { get; set; }
}

#endregion

/// <summary>
///     排序插件
/// </summary>
public abstract record SortSlot : ISortSlot
{
    /// <summary>
    ///     排序字段
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    ///     是否倒序
    /// </summary>
    public required OrderType Order { get; set; }
}

/// <summary>
///     排序定义
/// </summary>
public sealed record SortDefinition : SortSlot, ISortDefinition
{
    #region Implementation of ISortDefinition

    /// <summary>
    ///     顺序
    /// </summary>
    public required int Index { get; set; }

    #endregion
}