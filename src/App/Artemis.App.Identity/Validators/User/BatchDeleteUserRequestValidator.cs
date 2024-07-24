using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators.User;

/// <summary>
///     批量删除凭据请求验证
/// </summary>
public class BatchDeleteUserRequestValidator : AbstractValidator<BatchDeleteUserRequest>
{
    /// <summary>
    ///     构造
    /// </summary>
    public BatchDeleteUserRequestValidator()
    {
        RuleForEach(request => request.UserIds)
            .ShouldNotBeEmptyOrWhiteSpace()
            .ShouldBeGuid();
    }
}