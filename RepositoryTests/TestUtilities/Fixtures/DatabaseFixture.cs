using Dapper;
using Npgsql;

namespace RepositoryTests.TestUtilities.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgresSetting _postgresSetting;
    private readonly string _connectionString;

    public DatabaseFixture()
    {
        var settingProvider = new TestSettingProvider();
        _postgresSetting = settingProvider.PostgresSetting;
        _connectionString = string.Format(settingProvider.ConnectionString, _postgresSetting.HostPort,
            _postgresSetting.DatabaseName, _postgresSetting.User, _postgresSetting.Password);
    }
    
    public async Task InitializeAsync()
    {
        var path = Path.Combine("DatabaseScripts", "Database_pg13.sql");

        using var streamReader = new StreamReader(path);
        var sql = await streamReader.ReadToEndAsync();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql);
        await connection.CloseAsync();
    }

    public async Task DisposeAsync()
    {
        var sql = "DROP DATABASE IF EXISTS pg13";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql);
        await connection.CloseAsync();
    }
}