using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
/// 批量删除凭据请求验证
/// </summary>
public class BatchDeleteClaimRequestValidator : AbstractValidator<BatchDeleteClaimRequest>
{
    /// <summary>
    /// 构造
    /// </summary>
    public BatchDeleteClaimRequestValidator()
    {
        RuleForEach(request => request.ClaimIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}