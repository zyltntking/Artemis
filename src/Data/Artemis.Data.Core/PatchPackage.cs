using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
/// 修补项目接口
/// </summary>
/// <typeparam name="TPatch"></typeparam>
public interface IPatchItem<TPatch>
{
    /// <summary>
    /// 修补内容
    /// </summary>
    ICollection<TPatch> Items { get; set; }
}

#endregion

/// <summary>
///     基础修补协议
/// </summary>
[DataContract]
public record PatchPackage<TEntityPack, TKey>
{
    /// <summary>
    ///     批量添加
    /// </summary>
    [DataMember(Order = 1)]
    public PatchItem<TEntityPack>? AddPatches { get; set; }

    /// <summary>
    ///     批量删除
    /// </summary>
    [DataMember(Order = 2)]
    public PatchItem<KeyValuePair<TKey, TEntityPack>>? UpdatePatches { get; set; }

    /// <summary>
    ///     批量删除
    /// </summary>
    [DataMember(Order = 3)]
    public PatchItem<TKey>? RemovePatches { get; set; }
}

/// <summary>
///     修补项目
/// </summary>
/// <typeparam name="TPatch">修补数据类型</typeparam>
[DataContract]
public record PatchItem<TPatch> : IPatchItem<TPatch>
{
    /// <summary>
    ///     修补内容
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required ICollection<TPatch> Items { get; set; }
}