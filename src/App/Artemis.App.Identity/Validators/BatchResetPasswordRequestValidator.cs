using Artemis.Service.Protos.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     批量重置密码请求验证
/// </summary>
public class BatchResetPasswordRequestValidator : AbstractValidator<BatchResetPasswordRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchResetPasswordRequestValidator(IOptions<IdentityOptions> options)
    {
        RuleForEach(request => request.Batch).SetValidator(new ResetPasswordRequestValidator(options));
    }
}