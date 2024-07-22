using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
///     批量更新凭据请求验证
/// </summary>
public class BatchUpdateClaimRequestValidator : AbstractValidator<BatchUpdateClaimRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public BatchUpdateClaimRequestValidator()
    {
        RuleForEach(request => request.Batch)
            .SetValidator(new UpdateClaimRequestValidator());
    }
}