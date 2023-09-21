﻿using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Models;
using Artemis.Shared.Identity.Records;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Services.Identity.Managers;

/// <summary>
///     Artemis用户管理器接口
/// </summary>
public interface IIdentityManager : IManager<ArtemisUser>
{
    /// <summary>
    ///     根据角色名获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    Task<RoleInfo?> GetRoleAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色标识获取角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>角色实例</returns>
    /// <remarks>当查询不到角色实例时返回空</remarks>
    Task<RoleInfo?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色列表
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>角色列表</returns>
    /// <remarks>当查询不到角色实例时返回空列表</remarks>
    Task<IEnumerable<RoleInfo>> GetRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据角色名搜索角色
    /// </summary>
    /// <param name="nameSearch">角色名搜索值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken"></param>
    /// <returns>分页搜索结果</returns>
    /// <remarks>当查询不到角色实例时分页结果中数据集为空列表</remarks>
    Task<PageResult<RoleInfo>> FetchRolesAsync(
        string? nameSearch,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="description">角色描述</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>存储结果和创建成功的角色实例</returns>
    Task<StoreResult> CreateRoleAsync(
        string name,
        string? description = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleAsync(
        string name,
        RoleBase pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleAsync(
        Guid id,
        RoleBase pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateOrUpdateRoleAsync(
        string name,
        RoleBase pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建或更新角色
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="pack">角色信息</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateOrUpdateRoleAsync(
        Guid id,
        RoleBase pack,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<UserInfo>> FetchRoleUsersAsync(
        string name,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     根据用户名搜索具有该角色标签的用户
    /// </summary>
    /// <param name="id">角色标识</param>
    /// <param name="userNameSearch">用户名匹配值</param>
    /// <param name="emailSearch">邮箱匹配值</param>
    /// <param name="phoneSearch">电话匹配值</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<UserInfo>> FetchRoleUsersAsync(
        Guid id,
        string? userNameSearch = null,
        string? emailSearch = null,
        string? phoneSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色凭据列表
    /// </summary>
    /// <param name="name">角色名</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<RoleClaimInfo>> GetRoleClaimsAsync(
        string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取角色凭据列表
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<IEnumerable<RoleClaimInfo>> GetRoleClaimsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询角色的声明
    /// </summary>
    /// <param name="name">角色名称</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<RoleClaim>> FetchRoleClaimsAsync(
        string name,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     查询角色的声明
    /// </summary>
    /// <param name="id">角色id</param>
    /// <param name="claimTypeSearch">凭据类型</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面尺寸</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<PageResult<RoleClaim>> FetchRoleClaimsAsync(
        Guid id,
        string? claimTypeSearch = null,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleClaim">凭据</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateRoleClaimAsync(
        RoleClaim roleClaim,
        CancellationToken cancellationToken);

    /// <summary>
    ///     创建角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> CreateRoleClaimsAsync(
        IEnumerable<RoleClaim> roleClaims,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleClaim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleClaimAsync(
        RoleClaim roleClaim,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     更新角色凭据
    /// </summary>
    /// <param name="roleClaims">凭据列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> UpdateRoleClaimsAsync(
        IEnumerable<RoleClaim> roleClaims,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimId">角色凭据标识</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimAsync(
        int claimId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="roleId">角色标识</param>
    /// <param name="checkStamp">校验戳</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimAsync(
        Guid roleId,
        string checkStamp,
        CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimIds">凭据标识列表</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimsAsync(
        IEnumerable<int> claimIds,
        CancellationToken cancellationToken);

    /// <summary>
    ///     删除角色凭据
    /// </summary>
    /// <param name="claimKeys">角色键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    Task<StoreResult> DeleteRoleClaimsAsync(
        IEnumerable<ClaimKey> claimKeys,
        CancellationToken cancellationToken);
}