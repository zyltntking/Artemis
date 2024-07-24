using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量删除角色凭据请求验证
/// </summary>
public class BatchDeleteRoleClaimRequestValidator : AbstractValidator<BatchDeleteRoleClaimRequest>
{
    /// <summary>
    ///     批量删除角色凭据请求验证
    /// </summary>
    public BatchDeleteRoleClaimRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.ClaimIds)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}