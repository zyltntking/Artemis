using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     行政区划实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisDivisionConfiguration))]
public sealed class ArtemisDivision : Division
{
    /// <summary>
    ///     上级行政区划
    /// </summary>
    public ArtemisDivision? Parent { get; set; }


    /// <summary>
    ///     下级行政区划
    /// </summary>
    public ICollection<ArtemisDivision>? Children { get; set; }
}