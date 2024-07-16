namespace Exceptions.Base;

public class InvalidLoginException : CookBookException
{
    public InvalidLoginException(): base(ResourceMessageException.INVALID_LOGIN) {}
}