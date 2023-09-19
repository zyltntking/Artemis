namespace Artemis.Data.Core.Exceptions;

/// <summary>
///     实体查询失败异常
/// </summary>
public class EntityHasBeenSetException : ArtemisException
{
    /// <summary>
    ///     错误构造
    /// </summary>
    /// <param name="entityName"></param>
    /// <param name="flag"></param>
    /// <param name="errorCode">错误编码</param>
    public EntityHasBeenSetException(string entityName, string flag,
        int errorCode = ExceptionCode.EntityHasBeenSetException) : base($"插入实体:{entityName}异常, 查询flag:{flag}, 实体已经存在",
        errorCode)
    {
    }
}