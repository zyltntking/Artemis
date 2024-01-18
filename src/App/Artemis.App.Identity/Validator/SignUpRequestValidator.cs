using Artemis.Extensions.Web.Validators;
using Artemis.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validator;

/// <summary>
///     签入请求验证
/// </summary>
public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SignUpRequestValidator(IOptions<IdentityOptions> options)
    {
        var passwordOptions = options.Value.Password;

        RuleFor(request => request.Password).NotEmpty().WithMessage("不能为空");
        RuleFor(request => request.Password).MinimumLength(passwordOptions.RequiredLength)
            .WithMessage($"必须大于或等于{passwordOptions.RequiredLength}个字符");
        RuleFor(request => request.Password)
            .RequireLowerCase(passwordOptions.RequireLowercase)
            .RequireUpperCase(passwordOptions.RequireUppercase)
            .RequireDigit(passwordOptions.RequireDigit);

        RuleFor(request => request.UserName).NotEmpty().WithMessage("不能为空");
        RuleFor(request => request.PhoneNumber).NotEmpty().WithMessage("不能为空");
        RuleFor(request => request.Email).EmailAddress().WithMessage("不是有效的电子邮件地址");
    }
}