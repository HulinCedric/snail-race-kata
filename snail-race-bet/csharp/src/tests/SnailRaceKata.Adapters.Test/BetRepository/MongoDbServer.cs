using Testcontainers.MongoDb;

namespace SnailRaceKata.Test.Adapters.BetRepository;

public class MongoDbServer : IAsyncLifetime
{
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    public string GetConnectionString() => _container.GetConnectionString();
}