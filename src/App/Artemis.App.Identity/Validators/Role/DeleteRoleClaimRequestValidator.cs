using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     删除角色凭据请求验证
/// </summary>
public class DeleteRoleClaimRequestValidator : AbstractValidator<DeleteRoleClaimRequest>
{
    /// <summary>
    ///     删除角色凭据请求验证
    /// </summary>
    public DeleteRoleClaimRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}