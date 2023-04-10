namespace Artemis.Data.Store;

/// <summary>
///     Represents errors that occur during an interop call from .NET to MapTargetNull.
/// </summary>
public class MapTargetNullException : Exception
{
    /// <summary>
    ///     Constructs an instance of <see cref="MapTargetNullException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public MapTargetNullException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Constructs an instance of <see cref="MapTargetNullException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public MapTargetNullException(string message, Exception innerException) : base(message, innerException)
    {
    }
}