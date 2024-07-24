using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     添加用户角色请求验证
/// </summary>
public class AddUserRoleRequestValidator : AbstractValidator<AddUserRoleRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public AddUserRoleRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.Role)
            .NotNull()
            .SetValidator(new RoleIdPacketValidator());
    }
}