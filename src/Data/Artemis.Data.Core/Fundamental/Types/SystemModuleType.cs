using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 系统模块类型
/// </summary>
[Description("系统模块类型")]
public sealed class SystemModuleType : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private SystemModuleType(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// 模组
    /// </summary>
    [Description("模组")]
    public static SystemModuleType Module = new(1, nameof(Module));

    /// <summary>
    /// 界面
    /// </summary>
    [Description("界面")]
    public static SystemModuleType Interface = new(2, nameof(Interface));

    /// <summary>
    /// 操作
    /// </summary>
    [Description("操作")]
    public static SystemModuleType Action = new(3, nameof(Action));
}