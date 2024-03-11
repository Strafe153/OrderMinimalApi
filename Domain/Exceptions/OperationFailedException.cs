namespace Domain.Exceptions;

public class OperationFailedException : ApplicationException
{
	public OperationFailedException()
	{
	}

	public OperationFailedException(string message)
		: base(message)
	{
	}

	public OperationFailedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
