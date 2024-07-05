using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     查找用户角色请求验证
/// </summary>
public class UpdateUserClaimRequestValidator : AbstractValidator<UpdateUserClaimRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public UpdateUserClaimRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId).GreaterThanOrEqualTo(0);
    }
}