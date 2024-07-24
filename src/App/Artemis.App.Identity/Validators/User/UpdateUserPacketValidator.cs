using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     更新用户数据包验证
/// </summary>
public class UpdateUserPacketValidator : AbstractValidator<UpdateUserPacket>
{
    /// <summary>
    ///     构造
    /// </summary>
    public UpdateUserPacketValidator()
    {
        RuleFor(request => request.UserName)
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.UserName));

        RuleFor(request => request.PhoneNumber)
            .ShouldBePhone()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber));

        RuleFor(request => request.Email)
            .ShouldBeEmail()
            .ShouldShorterThan(128)
            .When(request => !string.IsNullOrWhiteSpace(request.Email));
    }
}