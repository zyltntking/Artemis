using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     更新角色凭据数据包验证
/// </summary>
public class UpdateRoleClaimPacketValidator : AbstractValidator<UpdateRoleClaimPacket>
{
    /// <summary>
    ///     更新角色凭据数据包验证
    /// </summary>
    public UpdateRoleClaimPacketValidator()
    {
        RuleFor(request => request.ClaimType)
            .ShouldShorterThan(128)
            .When(request => request.ClaimType != null);

        RuleFor(request => request.ClaimValue)
            .ShouldShorterThan(512)
            .When(request => request.ClaimValue != null);

        RuleFor(request => request.Description)
            .ShouldShorterThan(128)
            .When(request => request.Description != null);
    }
}