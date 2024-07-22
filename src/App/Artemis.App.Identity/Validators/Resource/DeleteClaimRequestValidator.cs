using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
/// 删除凭据请求验证
/// </summary>
public class DeleteClaimRequestValidator : AbstractValidator<DeleteClaimRequest>
{
    /// <summary>
    /// 构造
    /// </summary>
    public DeleteClaimRequestValidator()
    {
        RuleFor(request => request.ClaimId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}