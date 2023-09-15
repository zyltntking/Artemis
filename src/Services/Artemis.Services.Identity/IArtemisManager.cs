using Artemis.Data.Store;
using Artemis.Services.Identity.Data;
using Artemis.Shared.Identity.Models;

namespace Artemis.Services.Identity;

/// <summary>
/// Artemis用户管理器接口
/// </summary>
public interface IArtemisManager : IManager<ArtemisUser>
{
    /// <summary>
    /// 测试
    /// </summary>
    Task<AttachIdentityResult<Role>> Test();
}