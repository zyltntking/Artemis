using Artemis.Data.Core.Fundamental.Interface;
using Artemis.Data.Store;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Probe.Store;

/// <summary>
/// 元数据组
/// </summary>
public class MetadataGroup : StoreModel, IMeta
{
    #region Implementation of IMeta

    /// <summary>
    /// 数据键
    /// </summary>
    [Comment("元数据组键")]
    public  string Key { get; set; }

    /// <summary>
    /// 数据值
    /// </summary>
    [Comment("元数据组值")]
    public string Value { get; set; }

    #endregion
}