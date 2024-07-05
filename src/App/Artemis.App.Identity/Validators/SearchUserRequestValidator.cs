using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     查找用户请求验证
/// </summary>
public class SearchUserRequestValidator : AbstractValidator<SearchUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SearchUserRequestValidator()
    {
        RuleFor(request => request.UserName)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Phone)
            .ShouldBePhone()
            .When(request => !string.IsNullOrWhiteSpace(request.Phone));

        RuleFor(request => request.Email)
            .ShouldBeEmail()
            .When(request => !string.IsNullOrWhiteSpace(request.Email));

        RuleFor(request => request.Page).GreaterThanOrEqualTo(0)
            .When(request => request.Page != null);

        RuleFor(request => request.Size).GreaterThanOrEqualTo(0)
            .When(request => request.Size != null);
    }
}