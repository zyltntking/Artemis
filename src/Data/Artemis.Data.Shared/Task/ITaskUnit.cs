﻿namespace Artemis.Data.Shared.Task;

/// <summary>
///     任务单元接口
/// </summary>
public interface ITaskUnit
{
    /// <summary>
    ///     任务标识
    /// </summary>
    Guid TaskId { get; set; }

    /// <summary>
    ///     任务单元名称
    /// </summary>
    string UnitName { get; set; }

    /// <summary>
    ///     任务状态
    /// </summary>
    string TaskStatus { get; set; }

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