namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     实体查询失败异常
/// </summary>
public class EntityNotFoundException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="entityName"></param>
    /// <param name="flag"></param>
    /// <param name="errorCode">错误编码</param>
    public EntityNotFoundException(string entityName, string flag,
        int errorCode = ExceptionCode.EntityNotFoundException) : base($"查询实体:{entityName}异常, 查询flag:{flag}", errorCode)
    {
    }
}