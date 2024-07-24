using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量更新角色请求验证
/// </summary>
public class BatchUpdateRoleRequestValidator : AbstractValidator<BatchUpdateRoleRequest>
{
    /// <summary>
    ///     批量更新角色请求验证
    /// </summary>
    public BatchUpdateRoleRequestValidator()
    {
        RuleForEach(request => request.Batch)
            .NotNull()
            .SetValidator(new UpdateRoleRequestValidator());
    }
}