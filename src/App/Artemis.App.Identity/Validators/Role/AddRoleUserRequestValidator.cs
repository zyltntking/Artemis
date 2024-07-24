using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     添加角色用户请求验证
/// </summary>
public class AddRoleUserRequestValidator : AbstractValidator<AddRoleUserRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public AddRoleUserRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.User)
            .NotNull()
            .SetValidator(new UserIdPacketValidator());
    }
}