using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     批量删除角色用户请求验证
/// </summary>
public class BatchRemoveRoleUserRequestValidator : AbstractValidator<BatchRemoveRoleUserRequest>
{
    /// <summary>
    ///     批量删除角色用户请求验证
    /// </summary>
    public BatchRemoveRoleUserRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.UserIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}