using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Fundamental.Components;

/// <summary>
/// 电池信息接口
/// </summary>
/// <remarks><![CDATA[WMI class: Win32_Battery]]></remarks>
public class Battery : IBattery
{
    #region Implementation of IBattery

    /// <summary>
    /// 电池名
    /// </summary>
    public string BatteryName { get; set; } = string.Empty;

    /// <summary>
    /// 电池的完全充电容量（毫瓦时） 
    /// 将该值与 DesignCapacity 属性进行比较可确定何时需要更换电池。
    /// 电池的使用寿命通常在“满充电容量”属性低于“设计容量”属性的 80% 时结束。
    /// </summary>
    public uint FullChargeCapacity { get; set; }

    /// <summary>
    /// 电池的设计容量（毫瓦时）
    /// </summary>
    public uint DesignCapacity { get; set; }

    /// <summary>
    /// 电池的状态
    /// </summary>
    public ushort BatteryStatus { get; set; }

    /// <summary>
    /// 估计剩余电量的百分比
    /// </summary>
    public ushort EstimatedChargeRemaining { get; set; }

    /// <summary>
    /// 在当前负载条件下，如果市电关闭、丢失并保持关闭状态，或者笔记本电脑与电源断开连接，则以分钟为单位估计电池电量耗尽的时间
    /// </summary>
    public uint EstimatedRunTime { get; set; }

    /// <summary>
    /// 电池的预期使用寿命（分钟）
    /// 该属性表示电池的总预期寿命
    /// </summary>
    public uint ExpectedLife { get; set; }

    /// <summary>
    /// 电池充满电的最长时间（分钟）
    /// 该属性表示为完全耗尽的电池充电的时间
    /// </summary>
    public uint MaxRechargeTime { get; set; }

    /// <summary>
    /// 自计算机系统的UPS上次切换到电池电源以来经过的时间（秒），或自系统或UPS上次重新启动以来的时间，以较小者为准
    /// 如果电池“在线”，则返回 0（零）
    /// </summary>
    public uint TimeOnBattery { get; set; }

    /// <summary>
    /// 以当前充电速率和使用情况,为电池充满电的剩余时间（分钟）
    /// </summary>
    public uint TimeToFullCharge { get; set; }

    private string _batteryStatusDescription = string.Empty;

    /// <summary>
    ///  电池状态描述
    /// </summary>
    public string BatteryStatusDescription
    {
        get => !string.IsNullOrEmpty(_batteryStatusDescription) ? _batteryStatusDescription : BatteryStatus switch
        {
            1 => "电池正在放电",
            2 => "系统可以使用交流电，因此没有电池放电（电池不一定在充电）",
            3 => "充满电",
            4 => "电量低",
            5 => "危险",
            6 => "正在充电",
            7 => "正在充电（电量高）",
            8 => "正在充电（电量低）",
            9 => "正在充电（危险）",
            10 => "未安装电池",
            11 => "部分充电",
            _ => string.Empty
        };
        set => _batteryStatusDescription = value;
    }

    #endregion

    /// <summary>
    ///     元数据信息
    /// </summary>
    public ICollection<MetadataInfo>? Metadata { get; set; }

    #region Overrides of Object

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return
            $"{nameof(BatteryName)}: {BatteryName}" + Environment.NewLine +
            $"{nameof(FullChargeCapacity)}: {FullChargeCapacity}" + Environment.NewLine +
            $"{nameof(DesignCapacity)}: {DesignCapacity}" + Environment.NewLine +
            $"{nameof(BatteryStatusDescription)}: {BatteryStatusDescription}" + Environment.NewLine +
            $"{nameof(EstimatedChargeRemaining)}: {EstimatedChargeRemaining}" + Environment.NewLine +
            $"{nameof(EstimatedRunTime)}: {EstimatedRunTime}" + Environment.NewLine +
            $"{nameof(ExpectedLife)}: {ExpectedLife}" + Environment.NewLine +
            $"{nameof(MaxRechargeTime)}: {MaxRechargeTime}" + Environment.NewLine +
            $"{nameof(TimeOnBattery)}: {TimeOnBattery}" + Environment.NewLine +
            $"{nameof(TimeToFullCharge)}: {TimeToFullCharge}";

    }

    #endregion
}