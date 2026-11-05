namespace ONE_Classics_Nexus.Application.Abstractions.Services;

public interface IAutoNumberGenerator
{
    Task<string> NextAsync(
        string sequenceKey,
        string prefix,
        int width,
        CancellationToken cancellationToken);
}
