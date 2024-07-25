namespace Exceptions.Base;

public class NotFoundException : CookBookException
{
    public NotFoundException(string msg) : base(msg)
    {
    }
}