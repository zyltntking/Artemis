using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
///     批量创建凭据请求验证
/// </summary>
public class BatchCreateClaimRequestValidator : AbstractValidator<BatchCreateClaimRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public BatchCreateClaimRequestValidator()
    {
        RuleForEach(request => request.Batch)
            .SetValidator(new CreateClaimRequestValidator());
    }
}