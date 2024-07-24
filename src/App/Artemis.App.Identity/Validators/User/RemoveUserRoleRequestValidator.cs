using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     移除用户角色请求验证
/// </summary>
public class RemoveUserRoleRequestValidator : AbstractValidator<RemoveUserRoleRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public RemoveUserRoleRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}