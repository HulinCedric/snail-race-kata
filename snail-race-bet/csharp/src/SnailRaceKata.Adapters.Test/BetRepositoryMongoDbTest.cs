using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Test;

public class BetRepositoryMongoDbTest : BetRepositoryContractTest, IDisposable
{
    private readonly MongoClient _mongoClient = new("mongodb://localhost:27017");
    private readonly BetRepositoryMongoDb _repository;

    public BetRepositoryMongoDbTest()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Bet)))
            BsonClassMap.RegisterClassMap<Bet>(
                map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true); // Ignore missing _id field
                });

        var database = _mongoClient.GetDatabase("contract_testing");
        database.DropCollection("bet");

        _repository = new BetRepositoryMongoDb(database);
    }

    public void Dispose() => _mongoClient.Dispose();

    protected override BetRepository GetRepository() => _repository;
}