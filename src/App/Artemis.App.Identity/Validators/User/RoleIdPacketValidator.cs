using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
/// 角色标识数据包验证器
/// </summary>
public class RoleIdPacketValidator : AbstractValidator<RoleIdPacket>
{
    /// <summary>
    /// 构造
    /// </summary>
    public RoleIdPacketValidator()
    {
        RuleFor(request => request.RoleId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}