using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
///     创建凭据请求验证
/// </summary>
public class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public CreateClaimRequestValidator()
    {
        RuleFor(request => request.ClaimType)
            .ShouldShorterThan(128)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.ClaimValue)
            .ShouldShorterThan(512)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Description)
            .ShouldShorterThan(128)
            .When(request => request.Description != null);
    }
}