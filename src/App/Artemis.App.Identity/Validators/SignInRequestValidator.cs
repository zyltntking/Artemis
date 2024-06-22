namespace Artemis.App.Identity.Validators;

/// <summary>
///     签到请求验证
/// </summary>
//public class SignInRequestValidator : AbstractValidator<SignInRequest>
//{
//    /// <summary>
//    ///     验证器构造
//    /// </summary>
//    public SignInRequestValidator(IOptions<IdentityOptions> options)
//    {
//        var password = options.Value.Password;

//        RuleFor(request => request.Password)
//            .ShouldBePassword(
//                password.RequiredLength,
//                password.RequireDigit,
//                password.RequireUppercase,
//                password.RequireLowercase,
//                password.RequireNonAlphanumeric,
//                password.RequiredUniqueChars);

//        RuleFor(request => request.UserSign)
//            .ShouldNotBeEmptyOrWhiteSpace();
//    }
//}