using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Core;

#region interface

/// <summary>
///     过滤插件
/// </summary>
file interface IFilterSlot
{
    /// <summary>
    ///     过滤字段
    /// </summary>
    string Field { get; set; }

    /// <summary>
    ///     过滤操作
    /// </summary>
    FilterOperationType Operation { get; set; }

    /// <summary>
    ///     过滤值
    /// </summary>
    object? Value { get; set; }
}

#endregion

/// <summary>
///     过滤定义
/// </summary>
public record FilterDefinition : IFilterSlot
{
    #region Implementation of IFilterSlot

    /// <summary>
    ///     过滤字段
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    ///     过滤操作
    /// </summary>
    public required FilterOperationType Operation { get; set; }

    /// <summary>
    ///     过滤值
    /// </summary>
    public required object Value { get; set; }

    #endregion
}