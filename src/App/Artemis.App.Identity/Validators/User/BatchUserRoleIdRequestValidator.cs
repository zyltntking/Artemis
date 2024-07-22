using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     批量添加用户角色请求验证
/// </summary>
public class BatchUserRoleIdRequestValidator : AbstractValidator<BatchAddUserRoleRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchUserRoleIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleForEach(request => request.RoleIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}