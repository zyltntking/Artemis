using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     获取用户角色请求验证
/// </summary>
public class BatchUpdateUserClaimRequestValidator : AbstractValidator<BatchUpdateUserClaimRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchUpdateUserClaimRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.Batch)
            .NotNull()
            .SetValidator(new BatchUpdateUserClaimPacketValidator());
    }
}