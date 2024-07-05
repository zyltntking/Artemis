using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators;

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
            .ShouldBePassword(
                password.RequiredLength,
                password.RequireDigit,
                password.RequireUppercase,
                password.RequireLowercase,
                password.RequireNonAlphanumeric,
                password.RequiredUniqueChars);

        RuleFor(request => request.Sign.UserName)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Sign.Phone)
            .ShouldBePhone()
            .When(request => !string.IsNullOrWhiteSpace(request.Sign?.Phone));

        RuleFor(request => request.Sign.Email)
            .ShouldBeEmail()
            .When(request => !string.IsNullOrWhiteSpace(request.Sign?.Email));
    }
}