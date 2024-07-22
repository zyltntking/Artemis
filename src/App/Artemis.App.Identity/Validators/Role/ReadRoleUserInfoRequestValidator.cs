using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     读取角色用户信息请求验证
/// </summary>
public class ReadRoleUserInfoRequestValidator : AbstractValidator<ReadRoleUserInfoRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public ReadRoleUserInfoRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();

        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}