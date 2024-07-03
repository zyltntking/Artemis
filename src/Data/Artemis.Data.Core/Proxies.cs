namespace Artemis.Data.Core;

/// <summary>
///     操作者代理接口
/// </summary>
public interface IHandlerProxy : IHandlerProxy<Guid>;

/// <summary>
///     操作者代理接口
/// </summary>
public interface IHandlerProxy<out THandler> where THandler : IEquatable<THandler>
{
    /// <summary>
    ///     操作员
    /// </summary>
    THandler Handler { get; }
}

/// <summary>
///     抽象操作员代理实现
/// </summary>
public abstract class AbstractHandlerProxy : AbstractHandlerProxy<Guid>, IHandlerProxy;

/// <summary>
///     抽象操作员代理实现
/// </summary>
/// <typeparam name="THandler"></typeparam>
public abstract class AbstractHandlerProxy<THandler> : IHandlerProxy<THandler> where THandler : IEquatable<THandler>
{
    #region Implementation of IHandlerProxy<out THandler>

    /// <summary>
    ///     操作员
    /// </summary>
    public abstract THandler Handler { get; }

    #endregion
}