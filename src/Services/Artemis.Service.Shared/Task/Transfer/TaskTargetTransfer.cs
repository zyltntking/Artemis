namespace Artemis.Service.Shared.Task.Transfer;

/// <summary>
/// 任务目标信息
/// </summary>
public record TaskUnitTargetInfo : TaskUnitTargetPackage, ITaskUnitTargetInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     任务单元Id
    /// </summary>
    public Guid TaskUnitId { get; set; }
}

/// <summary>
/// 任务目标数据包
/// </summary>
public record TaskUnitTargetPackage : ITaskUnitTargetPackage
{
    /// <summary>
    ///     任务目标名称
    /// </summary>
    public required string TargetName { get; set; }

    /// <summary>
    ///     任务目标编码
    /// </summary>
    public string? TargetCode { get; set; }

    /// <summary>
    ///     设计编码
    /// </summary>
    public string? DesignCode { get; set; }

    /// <summary>
    ///     任务目标类型
    /// </summary>
    public required string TargetType { get; set; }

    /// <summary>
    ///     绑定标记
    /// </summary>
    public string? BindingTag { get; set; }

    /// <summary>
    ///     任务目标状态
    /// </summary>
    public required string TargetState { get; set; }

    /// <summary>
    ///     任务描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     任务目标执行
    /// </summary>
    public DateTime? ExecuteTime { get; set; }
}