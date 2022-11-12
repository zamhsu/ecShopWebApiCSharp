using System.IO;
using Microsoft.Extensions.Configuration;

namespace WebApiTests.TestUtilities;

public static class AppSettingProvider
{
    /// <summary>
    /// 取得測試的AppSettings
    /// </summary>
    /// <returns></returns>
    public static IConfiguration GetTestAppSettings()
    {
        var testAppSettings = Path.Combine("Settings", "appsettings.Test.json");
        var builder = new ConfigurationBuilder().AddJsonFile(testAppSettings);
        
        var config = builder.Build();
        return config;
    }
}