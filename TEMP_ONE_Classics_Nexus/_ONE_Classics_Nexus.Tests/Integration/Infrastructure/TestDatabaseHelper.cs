// add this helper to ensure a seeded vehicle type exists for tests
public async Task<Guid> EnsureDefaultVehicleTypeAsync(CancellationToken cancellationToken = default)
{
    const string sql = @"
        INSERT INTO vehicle_types (vehicle_type_id, name, created_at_utc, updated_at_utc)
        VALUES (@Id, @Name, @Now, @Now)
        ON DUPLICATE KEY UPDATE name = VALUES(name);";

    var id = Guid.Parse("00000000-0000-0000-0000-00000000DEFA"); // stable test GUID
    var now = DateTime.UtcNow;

    using var conn = await CreateOpenConnectionAsync(cancellationToken);
    await conn.ExecuteAsync(sql, new { Id = id.ToString(), Name = "Default", Now = now }, commandType: System.Data.CommandType.Text);

    return id;
}