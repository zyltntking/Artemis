namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     映射对象空异常
/// </summary>
public sealed class MapTargetNullException : ArtemisException
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="type">对象类型</param>
    public MapTargetNullException(Type type) : base($"映射对象{type.Name}为空", ExceptionCode.MapTargetNullException)
    {
    }

    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="message">The exception message.</param>
    public MapTargetNullException(string message) : base(message, ExceptionCode.MapTargetNullException)
    {
    }
}