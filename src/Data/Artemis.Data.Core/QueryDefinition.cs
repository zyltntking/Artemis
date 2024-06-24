namespace Artemis.Data.Core;

#region interface

/// <summary>
///     查询插件
/// </summary>
file interface IQuerySlot : IPageSlot
{
    /// <summary>
    ///     过滤定义
    /// </summary>
    ICollection<FilterDefinition>? FilterDefinitions { get; set; }

    /// <summary>
    ///     排序定义
    /// </summary>
    ICollection<SortDefinition>? SortDefinitions { get; set; }

    /// <summary>
    ///     是否启用分页
    /// </summary>
    bool UsePagination { get; set; }
}

#endregion

/// <summary>
///     查询定义
/// </summary>
public record QueryDefinition : PageSlot, IQuerySlot
{
    #region Implementation of IPageSlot

    /// <summary>
    ///     过滤定义
    /// </summary>
    public ICollection<FilterDefinition>? FilterDefinitions { get; set; }

    /// <summary>
    ///     排序定义
    /// </summary>
    public ICollection<SortDefinition>? SortDefinitions { get; set; }

    /// <summary>
    ///     是否启用分页
    /// </summary>
    public bool UsePagination { get; set; }

    #endregion
}