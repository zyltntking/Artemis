using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     查找用户角色请求验证
/// </summary>
public class SearchUserClaimInfoRequestValidator : AbstractValidator<SearchUserClaimInfoRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SearchUserClaimInfoRequestValidator()
    {
        RuleFor(request => request.UserId)
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