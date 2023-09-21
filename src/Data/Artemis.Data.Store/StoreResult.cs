using System.Globalization;
using System.Runtime.Serialization;

namespace Artemis.Data.Store;

/// <summary>
///     存储操作结果
/// </summary>
[DataContract]
public record StoreResult : IStoreResult
{
    private readonly List<StoreError> _errors = new();

    /// <summary>
    ///     指示操作是否成功的标志
    /// </summary>
    [DataMember(Order = 1)]
    public bool Succeeded { get; init; }

    /// <summary>
    ///     指示操作受影响行数
    /// </summary>
    [DataMember(Order = 2)]
    public int EffectRows { get; init; }

    /// <summary>
    ///     包含存储过程中产生的所有错误的实例
    /// </summary>
    [DataMember(Order = 3)]
    public IEnumerable<StoreError> Errors => _errors;

    /// <summary>
    ///     操作成功时返回结果
    /// </summary>
    /// <param name="effectRows">收影响行数</param>
    public static StoreResult Success(int effectRows)
    {
        return new StoreResult { Succeeded = true, EffectRows = effectRows };
    }

    /// <summary>
    ///     创建一个操作失败的实例
    /// </summary>
    /// <param name="errors">错误列表</param>
    /// <returns></returns>
    public static StoreResult Failed(params StoreError[]? errors)
    {
        var result = new StoreResult { Succeeded = false };
        if (errors is not null)
            result._errors.AddRange(errors);
        return result;
    }

    /// <summary>
    ///     ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Succeeded
            ? "Succeeded"
            : string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed",
                string.Join(",", Errors.Select(x => x.Code).ToList()));
    }
}

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
    int EffectRows { get; protected init; }

    /// <summary>
    ///     包含存储过程中产生的所有错误的实例
    /// </summary>
    IEnumerable<StoreError> Errors { get; }
}