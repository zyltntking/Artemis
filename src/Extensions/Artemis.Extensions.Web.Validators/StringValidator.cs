using Artemis.Data.Core.AscII;
using FluentValidation;

namespace Artemis.Extensions.Web.Validators;

/// <summary>
///     字符串验证器
/// </summary>
public static class StringValidator
{
    /// <summary>
    ///     必须包含小写字母
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> RequireLowerCase<T>(this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainLowerCase(input)).WithMessage("必须包含小写字符");
    }

    /// <summary>
    ///     必须包含大写字母
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> RequireUpperCase<T>(this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainUpperCase(input)).WithMessage("必须包含大写字符");
    }

    /// <summary>
    ///     必须包含数字字符
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> RequireDigit<T>(this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainNumber(input)).WithMessage("必须包含数字");
    }
}