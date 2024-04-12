using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     总线类型
/// </summary>
[Description("总线类型")]
public sealed class BusType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    [Description("未知类型")] public static BusType Unknown = new(0, nameof(Unknown));

    /// <summary>
    ///     数据总线
    /// </summary>
    [Description("数据总线")] public static BusType DataBus = new(1, nameof(DataBus));

    /// <summary>
    ///     控制总线
    /// </summary>
    [Description("控制总线")] public static BusType ControlBus = new(2, nameof(ControlBus));

    /// <summary>
    ///     事件总线
    /// </summary>
    [Description("事件总线")] public static BusType EventBus = new(3, nameof(EventBus));

    /// <summary>
    ///     扩展总线
    /// </summary>
    [Description("扩展总线")] public static BusType ExpansionBus = new(4, nameof(ExpansionBus));

    /// <summary>
    ///     局部总线
    /// </summary>
    [Description("局部总线")] public static BusType LocalBus = new(5, nameof(LocalBus));

    /// <summary>
    ///     业务总线
    /// </summary>
    [Description("业务总线")] public static BusType BusinessBus = new(6, nameof(BusinessBus));

    /// <summary>
    ///     服务总线
    /// </summary>
    [Description("服务总线")] public static BusType ServiceBuse = new(7, nameof(ServiceBuse));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private BusType(int id, string name) : base(id, name)
    {
    }
}