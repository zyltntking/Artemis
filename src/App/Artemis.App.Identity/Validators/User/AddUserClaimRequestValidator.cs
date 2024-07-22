using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     获取用户角色请求验证
/// </summary>
public class AddUserClaimRequestValidator : AbstractValidator<AddUserClaimRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public AddUserClaimRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.UserClaim)
            .NotNull()
            .SetValidator(new AddUserClaimPacketValidator());
    }
}