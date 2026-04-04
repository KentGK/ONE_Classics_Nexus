using System;

namespace ONE_Classics_Nexus.Application.Exceptions;

public sealed class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}