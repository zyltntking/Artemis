using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
///     更新凭据请求验证
/// </summary>
public class UpdateClaimRequestValidator : AbstractValidator<UpdateClaimRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public UpdateClaimRequestValidator()
    {
        RuleFor(request => request.ClaimId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.Claim)
            .NotNull()
            .SetValidator(new UpdateClaimPacketValidator());
    }
}