using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators.Account;

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
            .ShouldBePassword(
                password.RequiredLength,
                password.RequireDigit,
                password.RequireUppercase,
                password.RequireLowercase,
                password.RequireNonAlphanumeric,
                password.RequiredUniqueChars);

        RuleFor(request => request.UserName)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Phone)
            .ShouldBePhone()
            .When(request => !string.IsNullOrWhiteSpace(request.Phone));

        RuleFor(request => request.Email)
            .ShouldBeEmail()
            .When(request => !string.IsNullOrWhiteSpace(request.Email));
    }
}