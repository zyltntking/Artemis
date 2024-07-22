using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     批量移除用户角色请求验证
/// </summary>
public class BatchRemoveUserRoleRequestValidator : AbstractValidator<BatchRemoveUserRoleRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public BatchRemoveUserRoleRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.RoleIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}