namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     存储参数空异常
/// </summary>
public sealed class StoreParameterNullException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="parameterName">参数名</param>
    /// <param name="errorCode">错误编码</param>
    public StoreParameterNullException(string parameterName, int errorCode = ExceptionCode.StoreParameterNullException)
        : base($"传入存储的参数：{parameterName}为空", errorCode)
    {
    }
}