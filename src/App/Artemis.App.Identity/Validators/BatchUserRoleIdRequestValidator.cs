using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     获取用户角色请求验证
/// </summary>
public class BatchUserRoleIdRequestValidator : AbstractValidator<BatchUserRoleIdRequest>
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