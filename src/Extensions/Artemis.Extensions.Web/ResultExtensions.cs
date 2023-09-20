using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Artemis.Extensions.Web;

/// <summary>
///     结果扩展
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     从ModelState格式化失败结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="modelState">模型状态</param>
    /// <returns></returns>
    public static DataResult<T> Fail<T>(this ModelStateDictionary modelState)
    {
        var result = DataResult.Fail<T>();

        foreach (var (key, entry) in modelState)
        foreach (var entryError in entry.Errors)
            result.AddDescriptor(key, entryError.ErrorMessage);

        return result;
    }
}