﻿namespace Core.Exceptions;

public class OperationFailedException : ApplicationException
{
    public OperationFailedException(string message)
        : base(message)
    {
    }
}
