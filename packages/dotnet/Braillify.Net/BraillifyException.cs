namespace Braillify;

using System;

public sealed class BraillifyException : Exception
{
    public BraillifyException(string message) : base(message)
    {
    }

    public BraillifyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
