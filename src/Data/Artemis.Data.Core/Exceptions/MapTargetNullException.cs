namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     Represents errors that occur during an interop call from .NET to MapTargetNull.
/// </summary>
public class MapTargetNullException : ArtemisException
{
    /// <summary>
    ///     Constructs an instance of <see cref="MapTargetNullException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public MapTargetNullException(string message) : base(message, ExceptionCode.MapTargetNullException)
    {
    }
}