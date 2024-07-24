using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.Role;

/// <summary>
///     创建角色请求验证
/// </summary>
public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public CreateRoleRequestValidator()
    {
        RuleFor(request => request.RoleName)
            .ShouldShorterThan(128)
            .ShouldNotBeEmptyOrWhiteSpace();

        RuleFor(request => request.Description)
            .ShouldShorterThan(256)
            .When(request => !string.IsNullOrWhiteSpace(request.Description));
    }
}