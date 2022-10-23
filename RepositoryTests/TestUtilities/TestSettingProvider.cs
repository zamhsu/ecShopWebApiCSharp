using System.Text.Json;

namespace RepositoryTests.TestUtilities;

public class TestSettingProvider
{
    internal PostgresSetting PostgresSetting { get; private set; }
    internal string ConnectionString { get; private set; }
    private TestSetting _setting;
    
    public TestSettingProvider()
    {
        var path = @"TestSettings.json";

        using var streamReader = new StreamReader(path);
        var jsonString = streamReader.ReadToEnd();
        _setting = JsonSerializer.Deserialize<TestSetting>(jsonString);

        PostgresSetting = _setting.Postgres;
        ConnectionString = string.Format(_setting.ConnectionString, PostgresSetting.HostPort,
            PostgresSetting.DatabaseName, PostgresSetting.User, PostgresSetting.Password);
    }
}

public class TestSetting
{
    public PostgresSetting Postgres { get; set; }
    public string ConnectionString { get; set; }
}

public class PostgresSetting
{
    public string ImageName { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string DatabaseName  { get; set; }
    public string HostPort  { get; set; }
    public string ContainerPort  { get; set; }
}