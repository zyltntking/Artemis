using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     更新用户请求验证
/// </summary>
public class UserIdRequestValidator : AbstractValidator<UserIdRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public UserIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}