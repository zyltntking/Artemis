using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Identity.Protos;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     用户标识数据包验证
/// </summary>
public class UserIdPacketValidator : AbstractValidator<UserIdPacket>
{
    /// <summary>
    ///     用户标识数据包验证
    /// </summary>
    public UserIdPacketValidator()
    {
        RuleFor(request => request.UserId)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}