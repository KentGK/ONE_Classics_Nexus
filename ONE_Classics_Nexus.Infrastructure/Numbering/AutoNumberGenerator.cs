using Dapper;
using ONE_Classics_Nexus.Application.Abstractions.Services;
using ONE_Classics_Nexus.Infrastructure.Database;

namespace ONE_Classics_Nexus.Infrastructure.Numbering;

public sealed class AutoNumberGenerator(MySqlConnectionFactory factory) : IAutoNumberGenerator
{
    public async Task<string> NextAsync(
        string sequenceKey,
        string prefix,
        int width,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sequenceKey))
        {
            throw new ArgumentException("Sequence key is required.", nameof(sequenceKey));
        }

        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentException("Prefix is required.", nameof(prefix));
        }

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");
        }

        await using var connection = factory.Create();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string sql = """
            INSERT INTO entity_number_sequences(sequence_key, last_value)
            VALUES(@SequenceKey, 1)
            ON DUPLICATE KEY UPDATE last_value = LAST_INSERT_ID(last_value + 1);

            SELECT IF(ROW_COUNT() = 1, 1, LAST_INSERT_ID());
            """;

        var nextValue = await connection.ExecuteScalarAsync<long>(
            new CommandDefinition(
                sql,
                new { SequenceKey = sequenceKey.Trim() },
                transaction,
                cancellationToken: cancellationToken));

        await transaction.CommitAsync(cancellationToken);

        return $"{prefix.Trim()}_{nextValue.ToString($"D{width}")}";
    }
}
