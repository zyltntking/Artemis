using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
/// 读取用户凭据信息请求验证
/// </summary>
public class ReadUserClaimInfoRequestValidator : AbstractValidator<ReadUserClaimInfoRequest>
{
    /// <summary>
    /// 构造
    /// </summary>
    public ReadUserClaimInfoRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.ClaimId)
            .NotNull()
            .GreaterThan(0);
    }
}