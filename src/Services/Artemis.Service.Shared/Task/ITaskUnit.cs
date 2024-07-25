using Artemis.Data.Core;

namespace Artemis.Service.Shared.Task;

/// <summary>
///     任务单元接口
/// </summary>
public interface ITaskUnit : ITaskUnitInfo
{
    /// <summary>
    ///     标准化单元名
    /// </summary>
    string NormalizedUnitName { get; set; }
}

/// <summary>
///     任务单元信息接口
/// </summary>
public interface ITaskUnitInfo : ITaskUnitPackage, IKeySlot
{
    /// <summary>
    ///     任务标识
    /// </summary>
    Guid TaskId { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    string TaskUnitState { get; set; }

    /// <summary>
    ///     任务模式
    /// </summary>
    string TaskUnitMode { get; set; }
}

/// <summary>
///     任务单元数据包接口
/// </summary>
public interface ITaskUnitPackage
{
    /// <summary>
    ///     任务单元名称
    /// </summary>
    string UnitName { get; set; }

    /// <summary>
    ///     单元编码
    /// </summary>
    string? UnitCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    string? DesignCode { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     任务开始时间
    /// </summary>
    DateTime StartTime { get; set; }

    /// <summary>
    ///     任务结束时间
    /// </summary>
    DateTime? EndTime { get; set; }
}