namespace Artemis.Data.Store;

/// <summary>
///     存储子系统错误封装
/// </summary>
public record StoreError
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