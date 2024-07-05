using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     批量重置密码请求验证
/// </summary>
public class BatchResetPasswordRequestValidator : AbstractValidator<BatchResetPasswordRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchResetPasswordRequestValidator(IOptions<IdentityOptions> options)
    {
        var password = options.Value.Password;

        RuleForEach(request => request.Dictionary).ChildRules(reset =>
        {
            reset.RuleFor(request => request.UserId)
                .ShouldBeGuid();

            reset.RuleFor(request => request.Password)
                .ShouldBePassword(
                    password.RequiredLength,
                    password.RequireDigit,
                    password.RequireUppercase,
                    password.RequireLowercase,
                    password.RequireNonAlphanumeric,
                    password.RequiredUniqueChars);
        });
    }
}