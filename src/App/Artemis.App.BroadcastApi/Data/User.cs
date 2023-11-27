using Artemis.App.BroadcastApi.Data.Configuration;
using Artemis.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Artemis.App.BroadcastApi.Data;

/// <summary>
///     用户模型
/// </summary>
[EntityTypeConfiguration(typeof(UserConfiguration))]
public class User : ModelBase
{
    /// <summary>
    ///     用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    ///     规范化用户名
    /// </summary>
    public string NormalizedUserName { get; set; } = null!;

    /// <summary>
    ///     密码
    /// </summary>
    public string Password { get; set; } = null!;
}