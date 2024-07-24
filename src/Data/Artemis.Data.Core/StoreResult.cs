namespace Artemis.Data.Core;

/// <summary>
///     存储操作结果接口
/// </summary>
public interface IStoreResult
{
    /// <summary>
    ///     指示操作是否成功的标志
    /// </summary>
    bool Succeeded { get; protected init; }

    /// <summary>
    ///     指示操作受影响行数
    /// </summary>
    int AffectRows { get; protected init; }

    /// <summary>
    ///     包含存储过程中产生的所有错误的实例
    /// </summary>
    IEnumerable<StoreError> Errors { get; }

    /// <summary>
    ///    描述存储错误
    /// </summary>
    string DescribeError { get; }
}