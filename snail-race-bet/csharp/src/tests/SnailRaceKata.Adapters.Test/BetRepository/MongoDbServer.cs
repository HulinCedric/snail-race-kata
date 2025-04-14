using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace SnailRaceKata.Test.Adapters.BetRepository;

public class MongoDbServer : IAsyncLifetime
{
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();
    private readonly List<string> _initialDatabases = [];

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        using var mongoClient = new MongoClient(_container.GetConnectionString());

        _initialDatabases.AddRange(await AllDatabaseNames(mongoClient));
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();

    public string GetConnectionString() => _container.GetConnectionString();

    public async Task ResetAsync()
    {
        using var mongoClient = new MongoClient(_container.GetConnectionString());
        var databases = await AllDatabaseNames(mongoClient);

        foreach (var dbName in databases.Where(dbName => !_initialDatabases.Contains(dbName)))
        {
            await mongoClient.DropDatabaseAsync(dbName);
        }
    }

    private static async Task<List<string>> AllDatabaseNames(MongoClient mongoClient)
    {
        var databaseNames = await mongoClient.ListDatabaseNamesAsync();
        return await databaseNames.ToListAsync();
    }
}