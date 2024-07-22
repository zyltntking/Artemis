using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     更新角色请求验证
/// </summary>
public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public UpdateRoleRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.Role)
            .NotNull()
            .SetValidator(new UpdateRolePacketValidator());
    }
}