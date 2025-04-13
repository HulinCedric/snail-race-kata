using FluentAssertions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Test;

public class BetRepositoryMongoDbTest : IDisposable
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

    [Fact]
    public void Register_a_bet()
    {
        // Given
        var bet = new Bet("gambler", new PodiumPronostic(1, 2, 3), 12345);

        // When
        _repository.Register(bet);

        // Then
        _repository.FindByDateRange(12345, 12346)
            .Should()
            .BeEquivalentTo([bet]);
    }

    [Fact]
    public void Retrieve_only_bets_inside_the_time_range()
    {
        // Given
        var outerLowerRange = new Bet("gambler1", new PodiumPronostic(1, 2, 3), 12345);
        var equalToLowerRange = new Bet("gambler2", new PodiumPronostic(1, 2, 3), 12346);
        var inRange = new Bet("gambler3", new PodiumPronostic(1, 2, 3), 12347);
        var equalToUpperRange = new Bet("gambler4", new PodiumPronostic(1, 2, 3), 12348);
        var outerUpperRange = new Bet("gambler5", new PodiumPronostic(1, 2, 3), 12349);

        _repository.Register(outerLowerRange);
        _repository.Register(equalToLowerRange);
        _repository.Register(inRange);
        _repository.Register(equalToUpperRange);
        _repository.Register(outerUpperRange);

        // When
        var bets = _repository.FindByDateRange(12346, 12348);

        // Then
        bets.Should().BeEquivalentTo([equalToLowerRange, inRange, equalToUpperRange]);
    }
}

public class BetRepositoryFakeTest
{
    private readonly BetRepository _repository = new BetRepositoryFake();

    [Fact]
    public void Register_a_bet()
    {
        // Given
        var bet = new Bet("gambler", new PodiumPronostic(1, 2, 3), 12345);

        // When
        _repository.Register(bet);

        // Then
        _repository.FindByDateRange(12345, 12346)
            .Should()
            .BeEquivalentTo([bet]);
    }

    [Fact]
    public void Retrieve_only_bets_inside_the_time_range()
    {
        // Given
        var outerLowerRange = new Bet("gambler1", new PodiumPronostic(1, 2, 3), 12345);
        var equalToLowerRange = new Bet("gambler2", new PodiumPronostic(1, 2, 3), 12346);
        var inRange = new Bet("gambler3", new PodiumPronostic(1, 2, 3), 12347);
        var equalToUpperRange = new Bet("gambler4", new PodiumPronostic(1, 2, 3), 12348);
        var outerUpperRange = new Bet("gambler5", new PodiumPronostic(1, 2, 3), 12349);

        _repository.Register(outerLowerRange);
        _repository.Register(equalToLowerRange);
        _repository.Register(inRange);
        _repository.Register(equalToUpperRange);
        _repository.Register(outerUpperRange);

        // When
        var bets = _repository.FindByDateRange(12346, 12348);

        // Then
        bets.Should().BeEquivalentTo([equalToLowerRange, inRange, equalToUpperRange]);
    }
}