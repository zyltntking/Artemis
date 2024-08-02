namespace Artemis.Service.Shared.Task;

/// <summary>
///     任务目标接口
/// </summary>
public interface ITaskTarget
{
    /// <summary>
    ///     任务单元Id
    /// </summary>
    Guid TaskUnitId { get; set; }

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
    ///     任务目标外部标识
    /// </summary>
    string? TargetId { get; set; }

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