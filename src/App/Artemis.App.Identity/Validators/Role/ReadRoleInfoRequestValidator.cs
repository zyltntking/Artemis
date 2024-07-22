using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     读取角色信息请求验证
/// </summary>
public class ReadRoleInfoRequestValidator : AbstractValidator<ReadRoleInfoRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public ReadRoleInfoRequestValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}