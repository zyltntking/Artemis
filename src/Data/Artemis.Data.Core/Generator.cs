using Artemis.Data.Core.Exceptions;

namespace Artemis.Data.Core;

/// <summary>
///     实例辅助类
/// </summary>
public static class Generator
{
    /// <summary>
    ///     创建实例
    /// </summary>
    /// <typeparam name="TEntity">实例类型</typeparam>
    /// <returns>实例</returns>
    /// <exception cref="CreateInstanceException">创建实例异常</exception>
    public static TEntity CreateInstance<TEntity>()
    {
        try
        {
            return Activator.CreateInstance<TEntity>();
        }
        catch
        {
            throw new CreateInstanceException(nameof(TEntity));
        }
    }

    /// <summary>
    ///     判断是否继承
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TInterface">接口类型</typeparam>
    /// <returns></returns>
    public static bool IsInherit<TEntity>(Type type) where TEntity : class
    {
        return typeof(TEntity).GetInterfaces().Any(item => item == type);
    }
}