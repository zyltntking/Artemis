using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     添加角色凭据请求验证
/// </summary>
public class AddRoleClaimRequestValidator : AbstractValidator<AddRoleClaimRequest>
{
    /// <summary>
    ///     添加角色凭据请求验证
    /// </summary>
    public AddRoleClaimRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.RoleClaim)
            .NotNull()
            .SetValidator(new AddRoleClaimPacketValidator());
    }
}