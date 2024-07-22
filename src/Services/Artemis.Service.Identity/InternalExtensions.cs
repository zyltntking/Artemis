using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Identity.Protos;
using Mapster;

namespace Artemis.Service.Identity;

/// <summary>
/// 内部扩展
/// </summary>
internal static class InternalExtensions
{
    /// <summary>
    /// 存储结果转换为影响行数响应
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    internal static AffectedResponse AffectedResponse(this StoreResult result)
    {
        if (result.Succeeded)
        {
            var response = ResultAdapter.AdaptEmptySuccess<AffectedResponse>();
            response.Data = result.AffectRows;
            return response;
        }

        return ResultAdapter.AdaptEmptyFail<AffectedResponse>(result.DescribeError);
    }

    /// <summary>
    /// 分页结果转换为分页响应
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    internal static TResponse PagedResponse<TResponse, TData>(this PageResult<TData> result) 
    {
        return DataResult.Success(result).Adapt<TResponse>(PagedResultConfig);
    }

    /// <summary>
    /// 实体结果转换为读取实体响应
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static TResponse ReadInfoResponse<TResponse, TData>(this TData? data)
    {
        return data is null ? ResultAdapter.AdaptEmptyFail<TResponse>() : ResultAdapter.AdaptSuccess<TResponse, TData>(data);
    }

    /// <summary>
    /// 分页结果映射配置
    /// </summary>
    private static TypeAdapterConfig? _pagedResultConfig;

    /// <summary>
    /// 分页结果映射配置访问器
    /// </summary>
    private static TypeAdapterConfig PagedResultConfig
    {
        get
        {
            if (_pagedResultConfig == null)
            {
                _pagedResultConfig = new TypeAdapterConfig();

                _pagedResultConfig
                    .Default
                    .UseDestinationValue(member => member is { SetterModifier: AccessModifier.None, Type.IsGenericType: true } &&
                                                   member.Type.GetGenericTypeDefinition() == typeof(ICollection<>));
            }

            return _pagedResultConfig;
        }
    }
}