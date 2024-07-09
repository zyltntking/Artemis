using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     数据字典项实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisDataDictionaryItemConfiguration))]
public sealed class ArtemisDataDictionaryItem : DataDictionaryItem
{
    /// <summary>
    ///     数据字典
    /// </summary>
    public ArtemisDataDictionary DataDictionary { get; set; }
}