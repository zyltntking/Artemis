using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     数据字典实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisDataDictionaryConfiguration))]
public sealed class ArtemisDataDictionary : DataDictionary
{
    /// <summary>
    ///     数据字典项
    /// </summary>
    public ICollection<ArtemisDataDictionaryItem>? Items { get; set; }
}