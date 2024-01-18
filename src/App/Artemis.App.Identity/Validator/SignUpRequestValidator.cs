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
        var password = options.Value.Password;

        RuleFor(request => request.Password)
            .NotEmptyOrWhiteSpace()
            .RequireLength(password.RequiredLength)
            .RequireDigit(password.RequireDigit)
            .RequireUpperCase(password.RequireUppercase)
            .RequireLowerCase(password.RequireLowercase)
            .RequireNonAlphanumeric(password.RequireNonAlphanumeric)
            .RequiredUniqueChars(password.RequiredUniqueChars);

        RuleFor(request => request.UserName)
            .NotEmptyOrWhiteSpace();

        RuleFor(request => request.PhoneNumber)
            .ShouldBePhone()
            .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));

        RuleFor(request => request.Email)
            .ShouldBeEmail()
            .When(request => !string.IsNullOrWhiteSpace(request.Email));
    }
}