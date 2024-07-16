namespace Exceptions.Base;

public class ErrorOnValidationException : CookBookException
{
    public IList<string> ErrorsMessages { get; set; }

    public ErrorOnValidationException(IList<string> errors) : base(string.Empty)
    {
        ErrorsMessages = errors;
    }
}