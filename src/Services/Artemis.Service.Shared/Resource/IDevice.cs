namespace Artemis.Service.Shared.Resource;

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
    ///     设备序列号
    /// </summary>
    string SerialNumber { get; set; }

    /// <summary>
    ///     设备状态
    /// </summary>
    string Status { get; set; }

    /// <summary>
    ///     购买日期
    /// </summary>
    DateTime PurchaseDate { get; set; }

    /// <summary>
    ///     安装日期
    /// </summary>
    DateTime? InstallDate { get; set; }

    /// <summary>
    ///     保修日期
    /// </summary>
    DateTime? WarrantyDate { get; set; }

    /// <summary>
    ///     维护日期
    /// </summary>
    DateTime? MaintenanceDate { get; set; }

    /// <summary>
    ///     设备描述
    /// </summary>
    string? Description { get; set; }
}