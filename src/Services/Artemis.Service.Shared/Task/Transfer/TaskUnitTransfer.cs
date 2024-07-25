namespace Artemis.Service.Shared.Task.Transfer;

/// <summary>
///     任务单元信息
/// </summary>
public record TaskUnitInfo : TaskUnitPackage, ITaskUnitInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     任务标识
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    public required string TaskUnitState { get; set; }

    /// <summary>
    ///     任务模式
    /// </summary>
    public required string TaskUnitMode { get; set; }
}

/// <summary>
///     任务单元数据包
/// </summary>
public record TaskUnitPackage : ITaskUnitPackage
{
    /// <summary>
    ///     任务单元名称
    /// </summary>
    public required string UnitName { get; set; }

    /// <summary>
    ///     单元编码
    /// </summary>
    public string? UnitCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    public string? DesignCode { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     任务开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    ///     任务结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}