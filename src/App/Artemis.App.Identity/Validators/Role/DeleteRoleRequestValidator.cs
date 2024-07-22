using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     删除角色请求验证
/// </summary>
public class DeleteRoleRequestValidator : AbstractValidator<DeleteRoleRequest>
{
    /// <summary>
    ///     删除角色请求验证
    /// </summary>
    public DeleteRoleRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}