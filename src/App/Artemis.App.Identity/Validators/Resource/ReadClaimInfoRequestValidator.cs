using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Resource;

/// <summary>
///     读取凭据信息请求验证
/// </summary>
public class ReadClaimInfoRequestValidator : AbstractValidator<ReadClaimInfoRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public ReadClaimInfoRequestValidator()
    {
        RuleFor(request => request.ClaimId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}