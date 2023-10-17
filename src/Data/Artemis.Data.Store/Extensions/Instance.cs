using Artemis.Data.Core.Exceptions;
using Mapster;

namespace Artemis.Data.Store.Extensions;

/// <summary>
///     实例辅助类
/// </summary>
public static class Instance
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
    ///     创建实例
    /// </summary>
    /// <typeparam name="TEntity">实例类型</typeparam>
    /// <typeparam name="TMapEntity">映射实例类型</typeparam>
    /// <param name="mapEntity"></param>
    /// <returns>实例</returns>
    /// <exception cref="CreateInstanceException">创建实例异常</exception>
    public static TEntity CreateInstance<TEntity, TMapEntity>(TMapEntity mapEntity)
    {
        try
        {
            var entity = Activator.CreateInstance<TEntity>();

            mapEntity.Adapt(entity);

            return entity;
        }
        catch
        {
            throw new CreateInstanceException(nameof(TEntity));
        }
    }
}