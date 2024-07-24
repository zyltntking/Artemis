using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     添加用户凭据数据包验证
/// </summary>
public class AddUserClaimPacketValidator : AbstractValidator<AddUserClaimPacket>
{
    /// <summary>
    ///     构造
    /// </summary>
    public AddUserClaimPacketValidator()
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