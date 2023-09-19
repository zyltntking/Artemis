using Artemis.Data.Core;
using Artemis.Extensions.Web.Filter;
using Artemis.Shared.Identity;
using Artemis.Shared.Identity.Models;
using Grpc.Core;

namespace Artemis.App.Identity.Services;

/// <summary>
/// 角色服务
/// </summary>
public class RoleService : IRoleService
{
    #region Implementation of IRoleService

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="request">角色名</param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task<DataResult<Role>> GetRole(GetRoleRequest request, ServerCallContext? context = default)
    {
        var role = new Role
        {
            Name = request.Name,
            NormalizedName = "管理员",
            Description = "系统管理员"
        };

        return Task.FromResult(DataResult.Success(role));
    }

    #endregion
}