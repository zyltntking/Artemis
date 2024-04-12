using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     数据操作类型
/// </summary>
[Description("数据操作类型")]
public sealed class DataOperationType : Enumeration
{
    /// <summary>
    ///     未知操作
    /// </summary>
    [Description("未知操作")] public static DataOperationType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     添加数据
    /// </summary>
    [Description("添加数据")] public static DataOperationType Add = new(0, nameof(Add));

    /// <summary>
    ///     更新数据
    /// </summary>
    [Description("更新数据")] public static DataOperationType Update = new(1, nameof(Update));

    /// <summary>
    ///     删除数据
    /// </summary>
    [Description("删除数据")] public static DataOperationType Delete = new(2, nameof(Delete));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private DataOperationType(int id, string name) : base(id, name)
    {
    }
}