using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
/// 更新用户凭据数据包验证
/// </summary>
public class UpdateUserClaimPacketValidator : AbstractValidator<UpdateUserClaimPacket>
{
    /// <summary>
    /// 构造
    /// </summary>
    public UpdateUserClaimPacketValidator()
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