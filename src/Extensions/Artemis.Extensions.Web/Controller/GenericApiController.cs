using Artemis.Extensions.Web.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Extensions.Web.Controller;

/// <summary>
///     泛型凭据认证Api控制器
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
[ArtemisClaim]
public abstract class ClaimedGenericApiController : GenericApiController
{

}

/// <summary>
///     泛型Api控制器
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public abstract class GenericApiController : ControllerBase
{
}