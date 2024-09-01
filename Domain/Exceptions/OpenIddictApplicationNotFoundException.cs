namespace Domain.Exceptions;

public class OpenIddictApplicationNotFoundException : Exception
{
    public OpenIddictApplicationNotFoundException()
    {
    }

    public OpenIddictApplicationNotFoundException(string message)
        : base(message)
    {
    }

    public OpenIddictApplicationNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
