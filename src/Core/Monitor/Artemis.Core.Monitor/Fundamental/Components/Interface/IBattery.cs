namespace Artemis.Core.Monitor.Fundamental.Components.Interface;

/// <summary>
/// 电池信息接口
/// </summary>
/// <remarks><![CDATA[WMI class: Win32_Battery]]></remarks>
public interface IBattery
{
    /// <summary>
    /// 电池名
    /// </summary>
    string BatteryName { get; set; }

    /// <summary>
    /// 电池的完全充电容量（毫瓦时） 
    /// 将该值与 DesignCapacity 属性进行比较可确定何时需要更换电池。
    /// 电池的使用寿命通常在“满充电容量”属性低于“设计容量”属性的 80% 时结束。
    /// </summary>
    uint FullChargeCapacity { get; set; }

    /// <summary>
    /// 电池的设计容量（毫瓦时）
    /// </summary>
    uint DesignCapacity { get; set; }

    /// <summary>
    /// 电池的状态
    /// </summary>
    ushort BatteryStatus { get; set; }

    /// <summary>
    /// 估计剩余电量的百分比
    /// </summary>
    ushort EstimatedChargeRemaining { get; set; }

    /// <summary>
    /// 在当前负载条件下，如果市电关闭、丢失并保持关闭状态，或者笔记本电脑与电源断开连接，则以分钟为单位估计电池电量耗尽的时间
    /// </summary>
    uint EstimatedRunTime { get; set; }

    /// <summary>
    /// 电池的预期使用寿命（分钟）
    /// 该属性表示电池的总预期寿命
    /// </summary>
    uint ExpectedLife { get; set; }

    /// <summary>
    /// 电池充满电的最长时间（分钟）
    /// 该属性表示为完全耗尽的电池充电的时间
    /// </summary>
    uint MaxRechargeTime { get; set; }

    /// <summary>
    /// 自计算机系统的UPS上次切换到电池电源以来经过的时间（秒），或自系统或UPS上次重新启动以来的时间，以较小者为准
    /// 如果电池“在线”，则返回 0（零）
    /// </summary>
    uint TimeOnBattery { get; set; }

    /// <summary>
    /// 以当前充电速率和使用情况,为电池充满电的剩余时间（分钟）
    /// </summary>
    uint TimeToFullCharge { get; set; }

    /// <summary>
    ///  电池状态描述
    /// </summary>
    string BatteryStatusDescription { get; set; }
}