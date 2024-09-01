namespace Domain.Exceptions;

public class GrantNotImplementedException : Exception
{
    public GrantNotImplementedException()
    {
    }

    public GrantNotImplementedException(string message)
        : base(message)
    {
    }

    public GrantNotImplementedException(string message, Exception innterException)
        : base(message, innterException)
    {
    }
}
