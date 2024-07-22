using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     查找用户角色请求验证
/// </summary>
public class UserClaimIdRequestValidator : AbstractValidator<UpdateUserClaimRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public UserClaimIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId)
            .GreaterThan(0);

        RuleFor(request => request.UserClaim)
            .NotNull()
            .SetValidator(new UpdateUserClaimPacketValidator());
    }
}