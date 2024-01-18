using Artemis.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validator;

/// <summary>
///     签入请求验证
/// </summary>
public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SignUpRequestValidator()
    {
        RuleFor(request => request.UserName).NotEmpty();
        RuleFor(request => request.PhoneNumber).NotEmpty();
        RuleFor(request => request.Email).EmailAddress();
        RuleFor(request => request.Password).NotEmpty();
    }
}