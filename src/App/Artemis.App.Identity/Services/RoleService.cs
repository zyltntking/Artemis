using Artemis.Data.Core;
using Artemis.Extensions.Web.Controller;
using Artemis.Services.Identity;
using Artemis.Shared.Identity;
using Artemis.Shared.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.App.Identity.Services;

/// <summary>
/// 角色控制器
/// </summary>
[ControllerName("Role")]
public class RoleService : ApiController, IRoleService
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="logger"></param>
    public RoleService(IIdentityManager manager, ILogger<RoleService> logger) : base(logger)
    {
        IdentityManager = manager;
    }

    /// <summary>
    /// 认证管理器
    /// </summary>
    private IIdentityManager IdentityManager { get; }

    #region Implementation of IRoleService

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="request">角色名</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<DataResult<Role>> GetRole(GetRoleRequest request)
    {

        var result = await IdentityManager.GetRoleAsync(request.RoleId);

        if (result is not null)
        {
            return DataResult.Success(result);
        }

        return DataResult.Fail<Role>("未查询到匹配的角色");
    }

    #endregion

    //// GET: api/<RoleController>
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new[] { "value1", "value2" };
    //}

    //// GET api/<RoleController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<RoleController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<RoleController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<RoleController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}