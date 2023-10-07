using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     存储操作结果接口
/// </summary>
file interface IStoreResult
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

#endregion

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
    [Required]
    [DataMember(Order = 1)]
    public required bool Succeeded { get; init; }

    /// <summary>
    ///     指示操作受影响行数
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int EffectRows { get; init; }

    /// <summary>
    ///     包含存储过程中产生的所有错误的实例
    /// </summary>
    [DataMember(Order = 3)]
    public IEnumerable<StoreError> Errors => _errors;

    /// <summary>
    ///     描述器
    /// </summary>
    private static StoreErrorDescriber Describer => new();

    /// <summary>
    ///     描述错误代码
    /// </summary>
    /// <returns></returns>
    private string DescribeCode => string.Join(",", Errors.Select(error => error.Code).ToList());

    /// <summary>
    ///     描述错误
    /// </summary>
    public string DescribeError => string.Join(",", Errors.Select(error => error.Description).ToList());

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
        var result = new StoreResult
        {
            Succeeded = false,
            EffectRows = 0
        };
        if (errors is not null)
            result._errors.AddRange(errors);
        return result;
    }

    /// <summary>
    ///     实体已存在失败
    /// </summary>
    /// <param name="name">实体名</param>
    /// <param name="flag">判断标识</param>
    /// <returns></returns>
    public static StoreResult EntityFoundFailed(string name, string flag)
    {
        return Failed(Describer.EntityHasBeenSet(name, flag));
    }

    /// <summary>
    ///     实体不存在失败
    /// </summary>
    /// <param name="name">实体名</param>
    /// <param name="flag">判断标识</param>
    /// <returns></returns>
    public static StoreResult EntityNotFoundFailed(string name, string flag)
    {
        return Failed(Describer.EntityNotFound(name, flag));
    }

    /// <summary>
    ///     属性为空失败
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns></returns>
    public static StoreResult PropertyIsNullFailed(string? propertyName)
    {
        return Failed(Describer.PropertyIsNull(propertyName));
    }

    /// <summary>
    ///     ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Succeeded
            ? "Succeeded"
            : string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", DescribeCode);
    }
}