﻿using System.ComponentModel.DataAnnotations;
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
    [MaxLength(32)]
    public string UserName { get; set; } = null!;

    /// <summary>
    ///     规范化用户名
    /// </summary>
    [MaxLength(32)]
    public string NormalizedUserName { get; set; } = null!;

    /// <summary>
    ///     密码
    /// </summary>
    [MaxLength(128)]
    public string Password { get; set; } = null!;
}