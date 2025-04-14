using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnailRaceKata.Adapters.BetRepository;
using SnailRaceKata.Domain;
using Testcontainers.MongoDb;

namespace SnailRaceKata.Test.Adapters.BetRepository;

public class BetRepositoryMongoDbTest : BetRepositoryContractTest, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbServer = new MongoDbBuilder().Build();
    private MongoClient _mongoClient;

    private BetRepositoryMongoDb _repository;

    public async Task InitializeAsync()
    {
        await _mongoDbServer.StartAsync();

        if (!BsonClassMap.IsClassMapRegistered(typeof(Bet)))
            BsonClassMap.RegisterClassMap<Bet>(
                map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true); // Ignore missing _id field
                });

        _mongoClient = new MongoClient(_mongoDbServer.GetConnectionString());

        var database = _mongoClient.GetDatabase("contract_testing");
        await database.DropCollectionAsync("bet");

        _repository = new BetRepositoryMongoDb(database);
    }

    public async Task DisposeAsync()
    {
        _mongoClient.Dispose();

        await _mongoDbServer.DisposeAsync();
    }

    protected override Domain.BetRepository GetRepository() => _repository;
}