using System.Runtime.Serialization;

namespace Artemis.Data.Store;

/// <summary>
///     存储子系统错误封装
/// </summary>
[DataContract]
public record StoreError
{
    /// <summary>
    ///     错误编码
    /// </summary>
    [DataMember(Order = 1)]
    public string? Code { get; init; }

    /// <summary>
    ///     错误描述
    /// </summary>
    [DataMember(Order = 2)]
    public string? Description { get; init; }
}