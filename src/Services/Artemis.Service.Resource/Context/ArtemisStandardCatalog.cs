using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     标准目录实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStandardCatalogConfiguration))]
public class ArtemisStandardCatalog : StandardCatalog
{
    /// <summary>
    ///     标准目录项目
    /// </summary>
    public ICollection<ArtemisStandardItem>? Items { get; set; }
}