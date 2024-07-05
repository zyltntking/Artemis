using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     更新用户请求验证
/// </summary>
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public UpdateUserRequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotNull()
            .ShouldBeGuid();

        RuleFor(request => request.Sign.UserName)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Sign.Phone)
            .ShouldBePhone()
            .When(request => !string.IsNullOrWhiteSpace(request.Sign?.Phone));

        RuleFor(request => request.Sign.Email)
            .ShouldBeEmail()
            .When(request => !string.IsNullOrWhiteSpace(request.Sign?.Email));
    }
}