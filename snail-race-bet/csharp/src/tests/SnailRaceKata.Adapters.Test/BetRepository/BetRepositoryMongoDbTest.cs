using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnailRaceKata.Adapters.BetRepository;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Adapters.BetRepository;

public class BetRepositoryMongoDbTest(MongoDbServer mongoDbServer)
    : BetRepositoryContractTest, IAsyncLifetime, IClassFixture<MongoDbServer>
{
    private MongoClient _mongoClient;
    private BetRepositoryMongoDb _repository;

    public async Task InitializeAsync()
    {
        await mongoDbServer.ResetAsync();

        _mongoClient = new MongoClient(mongoDbServer.GetConnectionString());

        if (!BsonClassMap.IsClassMapRegistered(typeof(Bet)))
            BsonClassMap.RegisterClassMap<Bet>(
                map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true); // Ignore missing _id field
                });

        var database = _mongoClient.GetDatabase("contract_testing");

        _repository = new BetRepositoryMongoDb(database);
    }

    public Task DisposeAsync()
    {
        _mongoClient.Dispose();

        return Task.CompletedTask;
    }

    protected override Domain.BetRepository GetRepository() => _repository;
}