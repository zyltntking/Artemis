namespace Artemis.Extensions.Web.OpenApi.Attributes;

/// <summary>
///     慢接口最终操作状态
/// </summary>
public enum LongRunningOperationFinalStateVia
{
    /// <summary>
    ///     最终响应在header中指向的uri中提供
    /// </summary>
    Location,

    /// <summary>
    ///     最终响应在Uri中提供
    /// </summary>
    OriginalUri
}

/// <summary>
///     用于运行时间较长的操作
///     设置 x-ms-long-running-operation 时，也要设置 x-ms-long-running-operation-options
/// </summary>
/// <see href="https://github.com/Azure/azure-resource-manager-rpc/blob/master/v1.0/Addendum.md#asynchronous-operations">
///     Asynchronous
///     Operations.
/// </see>
/// <see href="https://github.com/Azure/autorest/tree/master/docs/extensions#x-ms-long-running-operation">x-ms-long-running-operation.</see>
[AttributeUsage(AttributeTargets.Method)]
public sealed class LongRunningOperationAttribute : Attribute
{
    /// <summary>
    ///     最终状态标识
    /// </summary>
    public LongRunningOperationFinalStateVia FinalStateVia { get; set; }
}