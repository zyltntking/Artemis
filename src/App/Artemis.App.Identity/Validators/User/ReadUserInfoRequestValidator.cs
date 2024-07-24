using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     读取用户信息请求验证
/// </summary>
public class ReadUserInfoRequestValidator : AbstractValidator<ReadUserInfoRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public ReadUserInfoRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}