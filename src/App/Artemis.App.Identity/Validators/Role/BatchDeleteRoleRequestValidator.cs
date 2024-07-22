using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量删除角色请求验证
/// </summary>
public class BatchDeleteRoleRequestValidator : AbstractValidator<BatchDeleteRoleRequest>
{
    /// <summary>
    ///     批量删除角色请求验证
    /// </summary>
    public BatchDeleteRoleRequestValidator()
    {
        RuleForEach(request => request.RoleIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}