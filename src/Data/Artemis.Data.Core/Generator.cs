using Artemis.Data.Core.Exceptions;
using Artemis.Data.Core.Fundamental.Kit;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Core;

/// <summary>
///     实例辅助类
/// </summary>
public static class Generator
{
    /// <summary>
    ///     生成并发戳
    /// </summary>
    public static string ConcurrencyStamp => Guid.NewGuid().ToString("D");

    /// <summary>
    ///     生成加密戳
    /// </summary>
    public static string SecurityStamp => Base32.GenerateBase32();

    /// <summary>
    ///     生成签名
    /// </summary>
    public static string Signature => Guid.NewGuid().ToString("N");

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
    /// <returns></returns>
    public static bool IsInherit<TEntity>(Type type) where TEntity : class
    {
        return typeof(TEntity).GetInterfaces().Any(item => item == type);
    }

    /// <summary>
    ///     生成检查戳
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string CheckStamp(string key)
    {
        return Hash.HashData(key, HashType.Md5);
    }

    /// <summary>
    ///     键值对摘要
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string PairSummary(string key, string value)
    {
        return $"{key}:{value}";
    }
}