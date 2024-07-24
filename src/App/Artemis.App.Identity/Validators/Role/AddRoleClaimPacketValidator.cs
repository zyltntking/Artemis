using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     添加角色凭据请求验证
/// </summary>
public class AddRoleClaimPacketValidator : AbstractValidator<AddRoleClaimPacket>
{
    /// <summary>
    ///     添加角色凭据请求验证
    /// </summary>
    public AddRoleClaimPacketValidator()
    {
        RuleFor(request => request.ClaimType)
            .ShouldShorterThan(128)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.ClaimValue)
            .ShouldShorterThan(512)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Description)
            .ShouldShorterThan(128)
            .When(request => request.Description != null);
    }
}