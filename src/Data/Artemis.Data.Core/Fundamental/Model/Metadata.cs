using Artemis.Data.Core.Fundamental.Interface;

namespace Artemis.Data.Core.Fundamental.Model;

/// <summary>
/// 元数据信息
/// </summary>
public class MetadataInfo : IMeta
{
    #region Implementation of IMeta

    /// <summary>
    /// 元数据键
    /// </summary>
    public virtual string Key { get; set; }

    /// <summary>
    /// 元数据值
    /// </summary>
    public virtual string Value { get; set; }

    #endregion
}

/// <summary>
/// 数据字典信息
/// </summary>
public class DataDictInfo : MetadataInfo, IDataDict
{
    #region Implementation of IDataDict

    /// <summary>
    /// 数据标签
    /// </summary>
    public virtual string Label { get; set; }

    /// <summary>
    /// 数据排序
    /// </summary>
    public virtual int Order { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public virtual string Type { get; set; }

    /// <summary>
    /// 数据是否锁定
    /// </summary>
    public virtual bool Lock { get; set; }

    /// <summary>
    /// 数据状态
    /// </summary>
    public virtual bool Status { get; set; }

    /// <summary>
    /// 数据描述
    /// </summary>
    public virtual string Description { get; set; }

    #endregion
}