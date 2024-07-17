using Application.SharedValidators;
using Communication.Requests;
using FluentValidation;

namespace Application.UseCases.User.ChangePassword;

public class ChangeUserPasswordValidator : AbstractValidator<RequestChangeUserPasswordJson>
{
    public ChangeUserPasswordValidator()
    {
        RuleFor(x => x.NewPassword)
            .SetValidator(new PasswordValidator<RequestChangeUserPasswordJson>());
    }
}
