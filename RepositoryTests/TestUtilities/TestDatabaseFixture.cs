using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace RepositoryTests.TestUtilities;

public class TestDatabaseFixture : IAsyncLifetime
{
    private readonly TestcontainersContainer _dbContainer;

    public TestDatabaseFixture()
    {
        _dbContainer = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("postgres:13.8")
            .WithEnvironment("POSTGRES_USER", "zamhsu")
            .WithEnvironment("POSTGRES_PASSWORD", "str0ngPassw0rD")
            .WithEnvironment("POSTGRES_DB", "pg13")
            .WithPortBinding("5432", "5432")
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
