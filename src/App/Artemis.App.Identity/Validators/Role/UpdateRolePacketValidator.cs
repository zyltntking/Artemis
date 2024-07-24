using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     更新角色数据包验证
/// </summary>
public class UpdateRolePacketValidator : AbstractValidator<UpdateRolePacket>
{
    /// <summary>
    ///     构造
    /// </summary>
    public UpdateRolePacketValidator()
    {
        RuleFor(request => request.RoleName)
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.RoleName));

        RuleFor(request => request.Description)
            .ShouldShorterThan(256)
            .When(request => !string.IsNullOrWhiteSpace(request.Description));
    }
}