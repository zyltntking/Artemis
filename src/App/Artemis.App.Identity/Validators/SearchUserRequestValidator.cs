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
        RuleFor(request => request.Search.UserName)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Search.Phone)
            .ShouldBePhone()
            .When(request => !string.IsNullOrWhiteSpace(request.Search?.Phone));

        RuleFor(request => request.Search.Email)
            .ShouldBeEmail()
            .When(request => !string.IsNullOrWhiteSpace(request.Search?.Email));

        RuleFor(request => request.Pagination.Page).GreaterThanOrEqualTo(0)
            .When(request => request.Pagination?.Page != null);

        RuleFor(request => request.Pagination.Size).GreaterThanOrEqualTo(0)
            .When(request => request.Pagination?.Size != null);
    }
}