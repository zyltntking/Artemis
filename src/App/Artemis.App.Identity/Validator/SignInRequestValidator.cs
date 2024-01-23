using Artemis.Extensions.Web.Validators;
using Artemis.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validator;

/// <summary>
///     签到请求验证
/// </summary>
public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SignInRequestValidator()
    {
        RuleFor(request => request.UserSign)
            .ShouldNotBeEmptyOrWhiteSpace();
    }
}