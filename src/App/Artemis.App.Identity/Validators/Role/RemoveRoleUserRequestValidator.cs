using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     移除角色用户请求验证
/// </summary>
public class RemoveRoleUserRequestValidator : AbstractValidator<RemoveRoleUserRequest>
{
    /// <summary>
    ///     移除角色用户请求验证
    /// </summary>
    public RemoveRoleUserRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}