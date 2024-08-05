using Artemis.Data.Core;

namespace Artemis.Service.Shared.Task;

/// <summary>
/// 任务目标接口
/// </summary>
public interface ITaskUnitTarget : ITaskUnitTargetInfo
{
}

/// <summary>
/// 任务目标信息接口
/// </summary>
public interface ITaskUnitTargetInfo : ITaskUnitTargetPackage, IKeySlot
{
    /// <summary>
    ///     任务单元Id
    /// </summary>
    Guid TaskUnitId { get; set; }
}

/// <summary>
///     任务目标数据包接口
/// </summary>
public interface ITaskUnitTargetPackage
{
    /// <summary>
    ///     任务目标名称
    /// </summary>
    string TargetName { get; set; }

    /// <summary>
    ///     任务目标编码
    /// </summary>
    string? TargetCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    string? DesignCode { get; set; }

    /// <summary>
    ///     任务目标类型
    /// </summary>
    string TargetType { get; set; }

    /// <summary>
    ///     绑定标记
    /// </summary>
    string? BindingTag { get; set; }

    /// <summary>
    ///     任务目标状态
    /// </summary>
    string TargetState { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     任务目标执行
    /// </summary>
    DateTime? ExecuteTime { get; set; }
}