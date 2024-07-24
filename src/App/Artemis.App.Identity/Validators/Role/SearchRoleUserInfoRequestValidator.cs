using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     查询角色用户信息请求验证
/// </summary>
public class SearchRoleUserInfoRequestValidator : AbstractValidator<SearchRoleUserInfoRequest>
{
    /// <summary>
    ///     查询角色用户信息请求验证
    /// </summary>
    public SearchRoleUserInfoRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.UserName)
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.UserName));

        RuleFor(request => request.PhoneNumber)
            .ShouldBePhone()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));

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