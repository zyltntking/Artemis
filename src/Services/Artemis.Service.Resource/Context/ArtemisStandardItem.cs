using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     标准项目实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStandardItemConfiguration))]
public class ArtemisStandardItem : StandardItem
{
    /// <summary>
    ///     标准项目所属目录
    /// </summary>
    public required ArtemisStandardCatalog Catalog { get; set; }
}