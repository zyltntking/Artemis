using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     结果数据附加集合
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加数据</typeparam>
file interface IAttachResult<TResult, TAttach>
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

#endregion

/// <summary>
///     结果数据附加集合记录
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加</typeparam>
public record AttachResult<TResult, TAttach> : IAttachResult<TResult, TAttach>
    where TResult : class
    where TAttach : class
{
    /// <summary>
    ///     结果
    /// </summary>
    [Required]
    public required TResult Result { get; init; }

    /// <summary>
    ///     附加数据
    /// </summary>
    [Required]
    public required TAttach Attach { get; init; }
}

/// <summary>
///     结果附加扩展
/// </summary>
public static class AttachResultExtensions
{
    /// <summary>
    ///     附加
    /// </summary>
    /// <typeparam name="TAttach">附加数据类型</typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="result">结果</param>
    /// <param name="attach">附加数据</param>
    /// <returns></returns>
    public static AttachResult<TResult, TAttach> Attach<TResult, TAttach>(
        this TResult result, TAttach attach)
        where TAttach : class 
        where TResult : class
    {
        return new AttachResult<TResult, TAttach>
        {
            Result = result,
            Attach = attach
        };
    }
}