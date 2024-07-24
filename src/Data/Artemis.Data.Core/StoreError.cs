namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     存储子系统错误接口
/// </summary>
file interface IStoreError
{
    /// <summary>
    ///     错误编码
    /// </summary>
    string? Code { get; init; }

    /// <summary>
    ///     错误描述
    /// </summary>
    string? Description { get; init; }
}

#endregion

/// <summary>
///     存储子系统错误封装
/// </summary>
public record StoreError : IStoreError
{
    /// <summary>
    ///     错误编码
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    ///     错误描述
    /// </summary>
    public string? Description { get; init; }
}