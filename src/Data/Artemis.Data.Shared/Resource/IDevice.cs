namespace Artemis.Data.Shared.Resource;

/// <summary>
///     设备表接口
/// </summary>
public interface IDevice
{
    /// <summary>
    ///     设备名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     设备类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     设备代码
    /// </summary>
    string Code { get; set; }

    /// <summary>
    ///     设备型号
    /// </summary>
    string Model { get; set; }

    /// <summary>
    ///     设备描述
    /// </summary>
    string? Description { get; set; }
}