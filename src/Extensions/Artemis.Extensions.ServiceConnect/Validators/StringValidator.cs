using Artemis.Data.Core.Fundamental;
using Artemis.Data.Core.Fundamental.AscII;
using FluentValidation;

namespace Artemis.Extensions.ServiceConnect.Validators;

/// <summary>
///     字符串验证器
/// </summary>
public static class StringValidator
{
    /// <summary>
    ///     不能为空,空字符串或纯空格字符
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> ShouldNotBeEmptyOrWhiteSpace<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("不能为空,空字符串或纯空格字符");
    }

    /// <summary>
    ///     必须大于或等于指定长度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> RequireLength<T>(
        this IRuleBuilder<T, string> ruleBuilder, int length)
    {
        return ruleBuilder.MinimumLength(length).WithMessage($"不得少于{length}个字符");
    }

    /// <summary>
    ///     必须包含数字字符
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> RequireDigit<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainNumber(input)).WithMessage("必须包含数字");
    }

    /// <summary>
    ///     必须包含大写字母
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> RequireUpperCase<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainUpperCase(input)).WithMessage("必须包含大写字符");
    }

    /// <summary>
    ///     必须包含小写字母
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> RequireLowerCase<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainLowerCase(input)).WithMessage("必须包含小写字符");
    }

    /// <summary>
    ///     必须包含特殊字符
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> RequireNonAlphanumeric<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        bool required = true)
    {
        return ruleBuilder.Must(input => !required || AsciiUtility.IsContainNonAlphanumeric(input))
            .WithMessage("必须包含特殊字符");
    }

    /// <summary>
    ///     需要的字符种类(唯一字符数)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> RequiredUniqueChars<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int count)
    {
        return ruleBuilder.Must(input => AsciiUtility.UniqueCharsCount(input) >= count);
    }

    /// <summary>
    ///     应当是有效的密码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="requireLength"></param>
    /// <param name="requireDigit"></param>
    /// <param name="requireUppercase"></param>
    /// <param name="requireLowercase"></param>
    /// <param name="requireNonAlphanumeric"></param>
    /// <param name="requiredUniqueChars"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> ShouldBePassword<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int requireLength,
        bool requireDigit,
        bool requireUppercase,
        bool requireLowercase,
        bool requireNonAlphanumeric,
        int requiredUniqueChars)
    {
        return ruleBuilder.ShouldNotBeEmptyOrWhiteSpace()
            .RequireLength(requireLength)
            .RequireDigit(requireDigit)
            .RequireUpperCase(requireUppercase)
            .RequireLowerCase(requireLowercase)
            .RequireNonAlphanumeric(requireNonAlphanumeric)
            .RequiredUniqueChars(requiredUniqueChars);
    }

    /// <summary>
    ///     应当是有效的电子邮件地址
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> ShouldBeEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.EmailAddress().WithMessage("不是有效的电子邮件地址");
    }

    /// <summary>
    ///     应当是有效的手机号码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> ShouldBePhone<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        PhonePatternMode mode = PhonePatternMode.Lax)
    {
        return ruleBuilder.Must(input => Matcher.PhoneMatcher(mode).IsMatch(input))
            .WithMessage("不是有效的手机号码");
    }

    /// <summary>
    ///     应当是有效的GUID
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="format"></param>
    /// <param name="caseMode"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> ShouldBeGuid<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        GuidFormat format = GuidFormat.D,
        CharacterCaseMode caseMode = CharacterCaseMode.Mix)
    {
        return ruleBuilder.Must(input => Matcher.GuidMatcher(format, caseMode).IsMatch(input))
            .WithMessage("不是有效的GUID标识");
    }
}