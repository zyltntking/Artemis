namespace Artemis.Data.Store;

/// <summary>
///     格式化资源
/// </summary>
public static class Formatter
{
    /// <summary>
    ///     格式化 未查询到Id为{0}的对象。 的本地化字符串。
    /// </summary>
    internal static string FormatNotFoundId(string? id)
    {
        return string.Format(Resources.NotFoundId, id);
    }

    /// <summary>
    ///     格式化未 查询到类型为{0}的对象。 的本地化字符串
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    internal static string FormatEntityNotFound(string? entity, string? flag)
    {
        return string.Format(Resources.EntityNotFound, entity, flag);
    }

    /// <summary>
    ///     格式化类型为{0}的对象已存在。 的本地化字符串
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    internal static string FormatEntityHasBeenSet(string? entity, string? flag)
    {
        return string.Format(Resources.EntityHasBeenSet, entity, flag);
    }

    /// <summary>
    ///     格式化属性{0}为空。 的本地化字符串
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <returns></returns>
    internal static string FormatPropertyIsNull(string? propertyName)
    {
        return string.Format(Resources.PropertyIsNull, propertyName);
    }
}