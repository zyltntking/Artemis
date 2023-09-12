using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity;

/// <summary>
/// Identity结果附加数据
/// </summary>
/// <typeparam name="T">附加数据类型</typeparam>
public class AttachResult<T> where T : class
{
    /// <summary>
    /// Identity结果
    /// </summary>
    public IdentityResult Result { get; set; } = null!;

    /// <summary>
    /// 附加数据
    /// </summary>
    public T Attach { get; set; } = default!;
}

/// <summary>
/// 结果附加扩展
/// </summary>
public static class AttachResultException
{
    /// <summary>
    /// 附加
    /// </summary>
    /// <typeparam name="T">附加数据类型</typeparam>
    /// <param name="result">结果</param>
    /// <param name="attach">附加数据</param>
    /// <returns></returns>
    public static AttachResult<T> Attach<T>(this IdentityResult result, T attach) where T : class
    {
        return new AttachResult<T>
        {
            Result = result,
            Attach = attach
        };
    }
}