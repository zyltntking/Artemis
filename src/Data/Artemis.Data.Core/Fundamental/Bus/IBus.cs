using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Core.Fundamental.Bus;

/// <summary>
///     总线接口
/// </summary>
public interface IBus
{
    /// <summary>
    ///     数据操作事件
    /// </summary>
    /// <param name="operation">操作类型</param>
    /// <param name="fieldName">域名称</param>
    /// <param name="entity">实体参数</param>
    void DataOperationAction(DataOperationType operation, string fieldName, object? entity);

    /// <summary>
    ///     事件触发事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="entity"></param>
    void EventRaiseAction(string eventName, object? entity);

    /// <summary>
    ///     服务调用事件
    /// </summary>
    /// <param name="serviceName">服务名</param>
    /// <param name="input">输入参数</param>
    /// <param name="output">输出参数</param>
    void ServiceCallAction(string serviceName, object? input, object? output);
}