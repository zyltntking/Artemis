using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     查找用户角色请求验证
/// </summary>
public class UserClaimIdRequestValidator : AbstractValidator<UserClaimIdRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public UserClaimIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId).GreaterThanOrEqualTo(0);
    }
}