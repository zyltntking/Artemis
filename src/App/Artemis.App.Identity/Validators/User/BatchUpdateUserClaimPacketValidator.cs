using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
/// 批量更新用户凭据数据包验证
/// </summary>
public class BatchUpdateUserClaimPacketValidator : AbstractValidator<BatchUpdateUserClaimPacket>
{
    /// <summary>
    /// 构造
    /// </summary>
    public BatchUpdateUserClaimPacketValidator()
    {
        RuleFor(request => request.ClaimId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(request => request.Claim)
            .NotNull()
            .SetValidator(new UpdateUserClaimPacketValidator());
    }
}