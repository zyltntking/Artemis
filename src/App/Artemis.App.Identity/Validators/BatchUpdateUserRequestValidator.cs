﻿using Artemis.Service.Protos.Identity;
using FluentValidation;

namespace Artemis.App.Identity.Validators;

/// <summary>
///     批量删除用户请求验证
/// </summary>
public class BatchUpdateUserRequestValidator : AbstractValidator<BatchUpdateUserRequest>
{
    /// <summary>
    ///     验证器构造
    /// </summary>
    public BatchUpdateUserRequestValidator()
    {
        RuleForEach(request => request.Batch).SetValidator(new UpdateUserRequestValidator());
    }
}