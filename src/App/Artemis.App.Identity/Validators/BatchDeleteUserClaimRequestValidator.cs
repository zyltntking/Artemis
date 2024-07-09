using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     获取用户角色请求验证
/// </summary>
public class BatchDeleteUserClaimRequestValidator : AbstractValidator<BatchDeleteUserClaimRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchDeleteUserClaimRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}