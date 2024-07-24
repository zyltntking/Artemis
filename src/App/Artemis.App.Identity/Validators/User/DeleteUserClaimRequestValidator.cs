using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     删除用户凭据请求验证
/// </summary>
public class DeleteUserClaimRequestValidator : AbstractValidator<DeleteUserClaimRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public DeleteUserClaimRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}