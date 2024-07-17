using Exceptions;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.SharedValidators;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";
    
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessageException.PASSWORD_INVALID);
            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
}