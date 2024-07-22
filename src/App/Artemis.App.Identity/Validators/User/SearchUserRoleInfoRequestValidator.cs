using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     查找用户角色请求验证
/// </summary>
public class SearchUserRoleInfoRequestValidator : AbstractValidator<SearchUserRoleInfoRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SearchUserRoleInfoRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.RoleName)
            .ShouldShorterThan(128)
            .When(request => request.RoleName != null);

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(0)
            .When(request => request.Page != null);

        RuleFor(request => request.Size)
            .GreaterThanOrEqualTo(0)
            .When(request => request.Size != null);
    }
}