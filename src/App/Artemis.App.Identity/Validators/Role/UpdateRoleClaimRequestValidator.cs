using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     更新角色凭据请求验证
/// </summary>
public class UpdateRoleClaimRequestValidator : AbstractValidator<UpdateRoleClaimRequest>
{
    /// <summary>
    ///     更新角色凭据请求验证
    /// </summary>
    public UpdateRoleClaimRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId)
            .GreaterThan(0);

        RuleFor(request => request.RoleClaim)
            .NotNull()
            .SetValidator(new UpdateRoleClaimPacketValidator());
    }
}