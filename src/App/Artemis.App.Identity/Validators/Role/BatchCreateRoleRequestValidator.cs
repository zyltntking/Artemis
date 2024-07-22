using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量创建角色请求验证
/// </summary>
public class BatchCreateRoleRequestValidator : AbstractValidator<BatchCreateRoleRequest>
{
    /// <summary>
    ///     批量创建角色请求验证
    /// </summary>
    public BatchCreateRoleRequestValidator()
    {
        RuleForEach(request => request.Batch)
            .SetValidator(new CreateRoleRequestValidator());
    }
}