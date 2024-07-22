using Artemis.Service.Identity.Protos;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     批量删除用户请求验证
/// </summary>
public class BatchCreateUserRequestValidator : AbstractValidator<BatchCreateUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchCreateUserRequestValidator(IOptions<IdentityOptions> options)
    {
        RuleForEach(request => request.Batch).SetValidator(new CreateUserRequestValidator(options));
    }
}