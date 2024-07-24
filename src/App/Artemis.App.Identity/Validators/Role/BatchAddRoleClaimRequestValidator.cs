using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量添加角色凭据请求验证
/// </summary>
public class BatchAddRoleClaimRequestValidator : AbstractValidator<BatchAddRoleClaimRequest>
{
    /// <summary>
    ///     批量添加角色凭据请求验证
    /// </summary>
    public BatchAddRoleClaimRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.Batch)
            .NotNull()
            .SetValidator(new AddRoleClaimPacketValidator());
    }
}