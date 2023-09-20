using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Records;

/// <summary>
///     结果数据附加集合记录
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加</typeparam>
[DataContract]
public record AttachResult<TResult, TAttach> : IAttachResult<TResult, TAttach>
    where TResult : class
    where TAttach : class
{
    /// <summary>
    ///     结果
    /// </summary>
    [DataMember(Order = 1)]
    public TResult Result { get; init; } = null!;

    /// <summary>
    ///     附加数据
    /// </summary>
    [DataMember(Order = 2)]
    public TAttach Attach { get; init; } = null!;
}

/// <summary>
///     结果数据附加集合
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加数据</typeparam>
public interface IAttachResult<TResult, TAttach>
    where TResult : class
    where TAttach : class
{
    /// <summary>
    ///     结果
    /// </summary>
    public TResult Result { get; init; }

    /// <summary>
    ///     附加数据
    /// </summary>
    public TAttach Attach { get; init; }
}

/// <summary>
///     结果附加扩展
/// </summary>
public static class AttachResultException
{
    /// <summary>
    ///     附加
    /// </summary>
    /// <typeparam name="TAttach">附加数据类型</typeparam>
    /// <param name="result">结果</param>
    /// <param name="attach">附加数据</param>
    /// <returns></returns>
    public static AttachResult<IdentityResult, TAttach> Attach<TAttach>(
        this IdentityResult result,
        TAttach attach)
        where TAttach : class
    {
        return new AttachResult<IdentityResult, TAttach>
        {
            Result = result,
            Attach = attach
        };
    }
}