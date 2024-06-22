using Artemis.Data.Core;
using Artemis.Data.Shared.Transfer.Identity;
using Artemis.Data.Store;
using Artemis.Service.Identity.Context;

namespace Artemis.Service.Identity.Managers;

/// <summary>
///     认证用户管理接口
/// </summary>
public interface IIdentityUserManager : IManager<IdentityUser, Guid, Guid>
{

}
