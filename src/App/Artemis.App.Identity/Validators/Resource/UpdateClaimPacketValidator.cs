using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
///     更新凭据数据包验证
/// </summary>
public class UpdateClaimPacketValidator : AbstractValidator<UpdateClaimPacket>
{
    /// <summary>
    ///     构造
    /// </summary>
    public UpdateClaimPacketValidator()
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