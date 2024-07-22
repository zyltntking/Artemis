using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量添加角色用户请求验证
/// </summary>
public class BatchAddRoleUserRequestValidator : AbstractValidator<BatchAddRoleUserRequest>
{
    /// <summary>
    ///     批量添加角色用户请求验证
    /// </summary>
    public BatchAddRoleUserRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.UserIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}