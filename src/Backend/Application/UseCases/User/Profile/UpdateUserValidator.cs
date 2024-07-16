using Communication.Requests;
using Exceptions;
using FluentValidation;

namespace Application.UseCases.User.Profile;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessageException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessageException.EMAIL_EMPTY);

        When(req => !string.IsNullOrWhiteSpace(req.Email), () =>
        {
            RuleFor(req => req.Email).EmailAddress()
                .WithMessage(ResourceMessageException.EMAIL_INVALID);
        });
    }
}