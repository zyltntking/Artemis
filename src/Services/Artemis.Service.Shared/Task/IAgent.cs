namespace Artemis.Service.Shared.Task;

/// <summary>
///     代理接口
/// </summary>
public interface IAgent
{
    /// <summary>
    ///     代理名称
    /// </summary>
    string AgentName { get; set; }

    /// <summary>
    ///     代理类型
    /// </summary>
    string AgentType { get; set; }

    /// <summary>
    ///     代理编码
    /// </summary>
    string AgentCode { get; set; }
}