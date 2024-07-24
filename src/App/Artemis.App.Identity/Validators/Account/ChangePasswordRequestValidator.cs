using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators.Account;

/// <summary>
///     修改密码请求验证
/// </summary>
public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public ChangePasswordRequestValidator(IOptions<IdentityOptions> options)
    {
        var password = options.Value.Password;

        RuleFor(request => request.NewPassword)
            .ShouldBePassword(
                password.RequiredLength,
                password.RequireDigit,
                password.RequireUppercase,
                password.RequireLowercase,
                password.RequireNonAlphanumeric,
                password.RequiredUniqueChars);

        RuleFor(request => request.OldPassword)
            .ShouldBePassword(
                password.RequiredLength,
                password.RequireDigit,
                password.RequireUppercase,
                password.RequireLowercase,
                password.RequireNonAlphanumeric,
                password.RequiredUniqueChars);
    }
}