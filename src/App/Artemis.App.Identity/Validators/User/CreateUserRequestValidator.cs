using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     创建用户请求验证
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public CreateUserRequestValidator(IOptions<IdentityOptions> options)
    {
        var password = options.Value.Password;

        RuleFor(request => request.Password)
            .ShouldShorterThan(128)
            .ShouldBePassword(
                password.RequiredLength,
                password.RequireDigit,
                password.RequireUppercase,
                password.RequireLowercase,
                password.RequireNonAlphanumeric,
                password.RequiredUniqueChars);

        RuleFor(request => request.UserName)
            .ShouldShorterThan(128)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.PhoneNumber)
            .ShouldBePhone()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));

        RuleFor(request => request.Email)
            .ShouldBeEmail()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.Email));
    }
}