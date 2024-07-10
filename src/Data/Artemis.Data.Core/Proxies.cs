namespace Artemis.Data.Core;

/// <summary>
///     操作者代理接口
/// </summary>
public interface IHandlerProxy
{
    /// <summary>
    ///     操作员
    /// </summary>
    string Handler { get; }
}

/// <summary>
///     抽象操作员代理实现
/// </summary>
public abstract class AbstractHandlerProxy : IHandlerProxy
{
    #region Implementation of IHandlerProxy

    /// <summary>
    ///     操作员
    /// </summary>
    public abstract string Handler { get; }

    #endregion
}