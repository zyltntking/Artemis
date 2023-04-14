namespace Artemis.Data.Core.Fundamental.Interface;

/// <summary>
///     元数据接口
/// </summary>
public interface IMetadata
{
    /// <summary>
    ///     数据键
    /// </summary>
    string Key { get; set; }

    /// <summary>
    ///     数据值
    /// </summary>
    string Value { get; set; }
}

/// <summary>
///     数据字典
/// </summary>
public interface IDataDict : IMetadata
{
    /// <summary>
    ///     数据标签
    /// </summary>
    string Label { get; set; }

    /// <summary>
    ///     数据排序
    /// </summary>
    int Order { get; set; }

    /// <summary>
    ///     数据类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     数据是否锁定
    /// </summary>
    bool Lock { get; set; }

    /// <summary>
    ///     数据状态
    /// </summary>
    bool Status { get; set; }

    /// <summary>
    ///     数据描述
    /// </summary>
    string Description { get; set; }
}