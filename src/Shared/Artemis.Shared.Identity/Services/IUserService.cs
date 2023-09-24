using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
using Artemis.Shared.Identity.Transfer;
using Grpc.Core;

namespace Artemis.Shared.Identity.Services;

/// <summary>
/// 用户服务接口
/// </summary>
[ServiceContract]
public interface IUserService
{
    /// <summary>
    /// 搜索用户
    /// </summary>
    /// <param name="request">查询用户请求</param>
    /// <param name="context">服务请求上下文</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    [OperationContract]
    Task<DataResult<PageResult<UserInfo>>> FetchUsersAsync(PageRequest<FetchUsersFilter> request, ServerCallContext? context = default, CancellationToken cancellationToken = default);
}

/// <summary>
/// 查询用户过滤器
/// </summary>
[DataContract]
public record FetchUsersFilter
{
    /// <summary>
    ///  用户名搜索值
    /// </summary>
    [DataMember(Order = 1)]
    public string? NameSearch { get; init; }

    /// <summary>
    /// 邮箱搜索值
    /// </summary>
    [DataMember(Order = 2)]
    public string? EmailSearch { get; init; }

    /// <summary>
    /// 电话号码搜索值
    /// </summary>
    [DataMember(Order = 3)]
    public string? PhoneNumberSearch { get; init; }
}