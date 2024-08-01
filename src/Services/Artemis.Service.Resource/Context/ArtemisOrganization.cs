using Artemis.Data.Core;
using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     组织机构实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisOrganizationConfiguration))]
public sealed class ArtemisOrganization : Organization, ITreeSlot<ArtemisOrganization>
{
    /// <summary>
    ///     上级机构
    /// </summary>
    public ArtemisOrganization? Parent { get; set; }

    /// <summary>
    ///     下级机构
    /// </summary>
    public ICollection<ArtemisOrganization>? Children { get; set; }
}