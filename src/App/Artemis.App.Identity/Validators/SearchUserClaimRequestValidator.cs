using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     查找用户角色请求验证
/// </summary>
public class SearchUserRoleRequestValidator : AbstractValidator<SearchUserRoleRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public SearchUserRoleRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.Page).GreaterThanOrEqualTo(0)
            .When(request => request.Page != null);

        RuleFor(request => request.Size).GreaterThanOrEqualTo(0)
            .When(request => request.Size != null);
    }
}