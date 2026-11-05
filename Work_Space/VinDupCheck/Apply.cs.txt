using MySqlConnector;

const string cs = "Server=localhost;Port=3309;Database=one_classics_nexus_test;User=root;Password=KentK1015;AllowUserVariables=true;";

await using var conn = new MySqlConnection(cs);
await conn.OpenAsync();

async Task<object?> ScalarAsync(string sql)
{
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = sql;
    return await cmd.ExecuteScalarAsync();
}

async Task ExecAsync(string sql)
{
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = sql;
    await cmd.ExecuteNonQueryAsync();
}

async Task ShowVinIndexesAsync(string label)
{
    Console.WriteLine(label);
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = "SHOW INDEX FROM vehicles WHERE Column_name = 'vin';";
    await using var r = await cmd.ExecuteReaderAsync();
    while (await r.ReadAsync())
        Console.WriteLine($"  name={r["Key_name"]}  non_unique={r["Non_unique"]}");
}

var dupes = Convert.ToInt64(await ScalarAsync(
    "SELECT COUNT(*) FROM (SELECT vin FROM vehicles WHERE vin IS NOT NULL AND TRIM(vin) <> '' GROUP BY vin HAVING COUNT(*) > 1) d;"));

if (dupes > 0)
{
    Console.WriteLine($"ABORT: {dupes} duplicate VIN group(s) present. Reconcile before adding the unique key.");
    return 1;
}

await ShowVinIndexesAsync("-- before --");

var hasUnique = Convert.ToInt64(await ScalarAsync(
    "SELECT COUNT(*) FROM information_schema.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'vehicles' AND INDEX_NAME = 'uk_vehicles_vin';")) > 0;

if (hasUnique)
{
    Console.WriteLine("\nuk_vehicles_vin already present; nothing to do.");
    return 0;
}

var hasOldIndex = Convert.ToInt64(await ScalarAsync(
    "SELECT COUNT(*) FROM information_schema.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'vehicles' AND INDEX_NAME = 'idx_vehicles_vin';")) > 0;

var sql = hasOldIndex
    ? "ALTER TABLE `vehicles` DROP INDEX `idx_vehicles_vin`, ADD UNIQUE KEY `uk_vehicles_vin` (`vin`);"
    : "ALTER TABLE `vehicles` ADD UNIQUE KEY `uk_vehicles_vin` (`vin`);";

Console.WriteLine($"\nApplying: {sql}");
await ExecAsync(sql);

Console.WriteLine();
await ShowVinIndexesAsync("-- after --");
return 0;
