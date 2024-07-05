using Artemis.Extensions.ServiceConnect.Validators;
using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     批量删除用户请求验证
/// </summary>
public class BatchDeleteUserRequestValidator : AbstractValidator<BatchDeleteUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchDeleteUserRequestValidator()
    {
        RuleForEach(request => request.UserIds).ShouldBeGuid();
    }
}