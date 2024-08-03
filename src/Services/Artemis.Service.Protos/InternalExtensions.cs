using Artemis.Data.Core;
using Google.Protobuf.Collections;
using Mapster;

namespace Artemis.Service.Protos;

/// <summary>
///     内部扩展
/// </summary>
public static class InternalExtensions
{
    /// <summary>
    ///     设置只读项目映射配置
    /// </summary>
    private static TypeAdapterConfig? _readOnlyCollectionSetConfig;

    /// <summary>
    ///     设置只读项目映射配置访问器
    /// </summary>
    private static TypeAdapterConfig ReadOnlyCollectionSetConfig
    {
        get
        {
            if (_readOnlyCollectionSetConfig == null)
            {
                _readOnlyCollectionSetConfig = new TypeAdapterConfig();

                _readOnlyCollectionSetConfig
                    .Default
                    .UseDestinationValue(member =>
                        member is { SetterModifier: AccessModifier.None, Type.IsGenericType: true } &&
                        member.Type.GetGenericTypeDefinition() == typeof(RepeatedField<>));
            }

            return _readOnlyCollectionSetConfig;
        }
    }

    /// <summary>
    ///     存储结果转换为影响行数响应
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static AffectedResponse AffectedResponse(this IStoreResult result)
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
    ///     分页结果转换为分页响应
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static TResponse PagedResponse<TResponse, TData>(this PageResult<TData> result)
    {
        return DataResult.Success(result).Adapt<TResponse>(ReadOnlyCollectionSetConfig);
    }

    /// <summary>
    ///     实体结果转换为读取实体响应
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static TResponse ReadInfoResponse<TResponse, TData>(this TData? data)
    {
        return data is null
            ? ResultAdapter.AdaptEmptyFail<TResponse>()
            : ResultAdapter.AdaptSuccess<TResponse, TData>(data, ReadOnlyCollectionSetConfig);
    }
}