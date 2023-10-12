using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 修补操作
/// </summary>
[Description("修补操作")]
public sealed class PatchAction : Enumeration
{
    /// <summary>
    /// 未知操作
    /// </summary>
    [Description("未知操作")]
    public static PatchAction Unknown = new(-1, nameof(Unknown));

    /// <summary>
    /// 添加操作
    /// </summary>
    [Description("添加操作")]
    public static PatchAction Add = new(0, nameof(Add));

    /// <summary>
    /// 删除操作
    /// </summary>
    [Description("删除操作")]
    public static PatchAction Remove = new(1, nameof(Remove));

    /// <summary>
    /// 替换操作
    /// </summary>
    [Description("替换操作")]
    public static PatchAction Replace = new(2, nameof(Replace));

    /// <summary>
    /// 更新操作
    /// </summary>
    [Description("更新操作")]
    public static PatchAction Update = new(3, nameof(Update));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private PatchAction(int id, string name) : base(id, name)
    {
    }
}