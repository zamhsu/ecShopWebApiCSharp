using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace RepositoryTests.TestUtilities.Fixtures;

public class DatabaseContainerFixture : IAsyncLifetime
{
    private readonly TestcontainersContainer _dbContainer;
    private readonly PostgresSetting _postgresSetting;

    public DatabaseContainerFixture()
    {
        var settingProvider = new TestSettingProvider();
        _postgresSetting = settingProvider.PostgresSetting;
        
        _dbContainer = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(_postgresSetting.ImageName)
            .WithEnvironment("POSTGRES_USER", _postgresSetting.User)
            .WithEnvironment("POSTGRES_PASSWORD", _postgresSetting.Password)
            .WithEnvironment("POSTGRES_DB", _postgresSetting.DatabaseName)
            .WithPortBinding(_postgresSetting.HostPort, _postgresSetting.ContainerPort)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    public async Task InitializeAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        await _dbContainer.StartAsync(cts.Token);
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
