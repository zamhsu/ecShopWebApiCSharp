using Dapper;
using Npgsql;

namespace RepositoryTests.TestUtilities.Fixtures;

public class AdminMemberTableFixture : IDisposable
{
    private readonly PostgresSetting _postgresSetting;
    private readonly string _connectionString;

    public AdminMemberTableFixture()
    {
        var settingProvider = new TestSettingProvider();
        _postgresSetting = settingProvider.PostgresSetting;
        _connectionString = string.Format(settingProvider.ConnectionString, _postgresSetting.HostPort,
            _postgresSetting.DatabaseName, _postgresSetting.User, _postgresSetting.Password);
        
        var path = Path.Combine("DatabaseScripts", "Table_AdminMember.sql");

        using var streamReader = new StreamReader(path);
        var sql = streamReader.ReadToEnd();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        connection.Execute(sql);
    }

    public void Dispose()
    {
        var sql = @"DROP TABLE IF EXISTS public.""AdminMember"";
DROP TABLE IF EXISTS public.""AdminMemberStatus"";";

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        connection.Execute(sql);
    }
}