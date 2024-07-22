using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     读取用户角色请求验证
/// </summary>
public class ReadUserRoleInfoRequestValidator : AbstractValidator<ReadUserRoleInfoRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public ReadUserRoleInfoRequestValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}