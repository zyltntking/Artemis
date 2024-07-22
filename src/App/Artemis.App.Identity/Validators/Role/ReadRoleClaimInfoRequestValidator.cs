using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     读取角色凭据信息请求验证
/// </summary>
public class ReadRoleClaimInfoRequestValidator : AbstractValidator<ReadRoleClaimInfoRequest>
{
    /// <summary>
    ///     读取角色凭据信息请求验证
    /// </summary>
    public ReadRoleClaimInfoRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId)
            .NotNull()
            .GreaterThan(0);
    }
}