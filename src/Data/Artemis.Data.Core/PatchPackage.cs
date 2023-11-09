using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     修补项目接口
/// </summary>
/// <typeparam name="TPatch"></typeparam>
public interface IPatchItem<TPatch>
{
    /// <summary>
    ///     修补内容
    /// </summary>
    ICollection<TPatch> Items { get; set; }
}

#endregion

/// <summary>
///     基础修补协议
/// </summary>
public record PatchPackage<TEntityPack, TKey>
{
    /// <summary>
    ///     批量添加
    /// </summary>
    public PatchItem<TEntityPack>? AddPatches { get; set; }

    /// <summary>
    ///     批量删除
    /// </summary>
    public PatchItem<KeyValuePair<TKey, TEntityPack>>? UpdatePatches { get; set; }

    /// <summary>
    ///     批量删除
    /// </summary>
    public PatchItem<TKey>? RemovePatches { get; set; }
}

/// <summary>
///     修补项目
/// </summary>
/// <typeparam name="TPatch">修补数据类型</typeparam>
public record PatchItem<TPatch> : IPatchItem<TPatch>
{
    /// <summary>
    ///     修补内容
    /// </summary>
    [Required]
    public required ICollection<TPatch> Items { get; set; }
}