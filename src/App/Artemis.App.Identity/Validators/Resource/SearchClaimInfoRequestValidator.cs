using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
/// 查询凭据信息请求验证
/// </summary>
public class SearchClaimInfoRequestValidator : AbstractValidator<SearchClaimInfoRequest>
{
    /// <summary>
    /// 构造
    /// </summary>
    public SearchClaimInfoRequestValidator()
    {
        RuleFor(request => request.ClaimType)
            .ShouldShorterThan(128)
            .When(request => request.ClaimType != null);

        RuleFor(request => request.ClaimValue)
            .ShouldShorterThan( 512)
            .When(request => request.ClaimValue != null);

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(0)
            .When(request => request.Page != null);

        RuleFor(request => request.Size)
            .GreaterThanOrEqualTo(0)
            .When(request => request.Size != null);
    }
}