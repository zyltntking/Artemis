using Artemis.Data.Core;
using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
/// 系统模块实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisSystemModuleConfiguration))]
public sealed class ArtemisSystemModule : SystemModule, ITreeSlot<ArtemisSystemModule>
{
    /// <summary>
    ///     下级模块
    /// </summary>
    public ICollection<ArtemisSystemModule>? Children { get; set; }

    /// <summary>
    ///     上级模块
    /// </summary>
    public ArtemisSystemModule? Parent { get; set; }
}