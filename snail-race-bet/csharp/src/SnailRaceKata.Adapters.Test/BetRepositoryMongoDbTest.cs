using MongoDB.Driver;

namespace SnailRaceKata.Adapters.Test;

public class BetRepositoryMongoDbTest : IDisposable
{
    private readonly MongoClient _mongoClient = new("mongodb://localhost:27017");
    private readonly BetRepositoryMongoDb _repository;

    public BetRepositoryMongoDbTest()
    {
        var database = _mongoClient.GetDatabase("contract_testing");
        database.DropCollection("bet");

        _repository = new BetRepositoryMongoDb(database);
    }

    public void Dispose() => _mongoClient.Dispose();

    [Fact(Skip = "TODO : write the test, then the implementation😉")]
    public void Register_a_bet()
    {
        // register a bet
        // How can we verify the bet is actually inserted ?
        // "Rien n'est plus dangereux qu'une idée quand on en a qu'une" (Emile Chartier)
        // TIPS : Find some (>=3) options before commiting to one
    }

    [Fact(Skip = "TODO : write the test, then the implementation😉")]
    public void Retrieve_only_bets_inside_the_time_range()
    {
    }
}