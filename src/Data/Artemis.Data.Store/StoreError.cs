namespace Artemis.Data.Store;

/// <summary>
///     存储子系统错误封装
/// </summary>
public class StoreError
{
    /// <summary>
    ///     错误编码
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    ///     错误描述
    /// </summary>
    public string Description { get; set; } = default!;
}