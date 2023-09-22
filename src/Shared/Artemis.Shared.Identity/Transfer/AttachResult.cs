using Artemis.Data.Core;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Shared.Identity.Transfer;

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