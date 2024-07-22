using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     查询角色声凭据息请求验证
/// </summary>
public class SearchRoleClaimInfoRequestValidator : AbstractValidator<SearchRoleClaimInfoRequest>
{
    /// <summary>
    ///     查询角色声凭据息请求验证
    /// </summary>
    public SearchRoleClaimInfoRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimType)
            .ShouldShorterThan(128)
            .When(request => request.ClaimType != null);

        RuleFor(request => request.ClaimValue)
            .ShouldShorterThan(512)
            .When(request => request.ClaimValue != null);

        RuleFor(request => request.Page).GreaterThanOrEqualTo(0)
            .When(request => request.Page != null);

        RuleFor(request => request.Size).GreaterThanOrEqualTo(0)
            .When(request => request.Size != null);
    }
}