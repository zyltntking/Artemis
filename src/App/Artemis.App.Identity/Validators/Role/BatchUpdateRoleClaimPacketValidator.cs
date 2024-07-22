using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量更新角色声明数据包验证
/// </summary>
public class BatchUpdateRoleClaimPacketValidator : AbstractValidator<BatchUpdateRoleClaimPacket>
{
    /// <summary>
    ///     批量更新角色声明数据包验证
    /// </summary>
    public BatchUpdateRoleClaimPacketValidator()
    {
        RuleFor(request => request.ClaimId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(request => request.Claim)
            .NotNull()
            .SetValidator(new UpdateRoleClaimPacketValidator());
    }
}