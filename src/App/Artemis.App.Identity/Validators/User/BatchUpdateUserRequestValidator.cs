using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     批量删除用户请求验证
/// </summary>
public class BatchUpdateUserRequestValidator : AbstractValidator<BatchUpdateUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchUpdateUserRequestValidator()
    {
        RuleForEach(request => request.Batch).SetValidator(new UpdateUserRequestValidator());
    }
}