using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     查询角色信息请求验证器
/// </summary>
public class SearchRoleInfoRequestValidator : AbstractValidator<SearchRoleInfoRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public SearchRoleInfoRequestValidator()
    {
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