using Artemis.Data.Core.Exceptions;

namespace Artemis.Data.Core;

/// <summary>
///     实例辅助类
/// </summary>
public static class Instance
{
    /// <summary>
    ///     创建实例
    /// </summary>
    /// <typeparam name="T">实例类型</typeparam>
    /// <returns></returns>
    /// <exception cref="CreateInstanceException">创建实例异常</exception>
    public static T CreateInstance<T>()
    {
        try
        {
            return Activator.CreateInstance<T>();
        }
        catch
        {
            throw new CreateInstanceException(nameof(T));
        }
    }
}