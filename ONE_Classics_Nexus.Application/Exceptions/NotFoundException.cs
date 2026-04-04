using System;

namespace ONE_Classics_Nexus.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}