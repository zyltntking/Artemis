using Artemis.Data.Store;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Services.Identity;

/// <summary>
/// Store结果附加数据
/// </summary>
/// <typeparam name="TAttach"></typeparam>
public record AttachStoreResult<TAttach> : AttachResult<StoreResult, TAttach> where TAttach : class;

/// <summary>
///     Identity结果附加数据
/// </summary>
/// <typeparam name="TAttach">附加数据类型</typeparam>
public record AttachIdentityResult<TAttach> : AttachResult<IdentityResult, TAttach> where TAttach : class;

/// <summary>
/// 结果附加
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加</typeparam>
public record AttachResult<TResult, TAttach> 
    where TResult : class 
    where TAttach : class
{
    /// <summary>
    /// 结果
    /// </summary>
    public TResult Result { get; init; } = null!;

    /// <summary>
    /// 附加
    /// </summary>
    public TAttach Attach { get; init; } = null!;
}

/// <summary>
///     结果附加扩展
/// </summary>
public static class AttachResultException
{
    /// <summary>
    /// 附加
    /// </summary>
    /// <typeparam name="TAttach"></typeparam>
    /// <param name="result"></param>
    /// <param name="attach"></param>
    /// <returns></returns>
    public static AttachStoreResult<TAttach> Attach<TAttach>(this StoreResult result, TAttach attach)
        where TAttach : class
    {
        return new AttachStoreResult<TAttach>
        {
            Result = result,
            Attach = attach
        };
    }

    /// <summary>
    ///     附加
    /// </summary>
    /// <typeparam name="TAttach">附加数据类型</typeparam>
    /// <param name="result">结果</param>
    /// <param name="attach">附加数据</param>
    /// <returns></returns>
    public static AttachIdentityResult<TAttach> Attach<TAttach>(this IdentityResult result, TAttach attach) 
        where TAttach : class
    {
        return new AttachIdentityResult<TAttach>
        {
            Result = result,
            Attach = attach
        };
    }
}