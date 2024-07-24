using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     查找用户请求验证
/// </summary>
public class SearchUserInfoRequestValidator : AbstractValidator<SearchUserInfoRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SearchUserInfoRequestValidator()
    {
        RuleFor(request => request.UserName)
            .ShouldShorterThan(128)
            .When(request => request.UserName != null);

        RuleFor(request => request.Phone)
            .ShouldBePhone()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.Phone));

        RuleFor(request => request.Email)
            .ShouldBeEmail()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.Email));

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(0)
            .When(request => request.Page != null);

        RuleFor(request => request.Size)
            .GreaterThanOrEqualTo(0)
            .When(request => request.Size != null);
    }
}