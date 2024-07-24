using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators.Account;

/// <summary>
///     签到请求验证
/// </summary>
public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SignInRequestValidator(IOptions<IdentityOptions> options)
    {
        var password = options.Value.Password;

        RuleFor(request => request.Password)
            .ShouldBePassword(
                password.RequiredLength,
                password.RequireDigit,
                password.RequireUppercase,
                password.RequireLowercase,
                password.RequireNonAlphanumeric,
                password.RequiredUniqueChars);

        RuleFor(request => request.UserSign)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.EndType.ToString("G"))
            .ShouldNotBeEmptyOrWhiteSpace();
    }
}