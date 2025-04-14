using MongoDB.Driver;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.BetRepository;

public class BetRepositoryMongoDb(IMongoDatabase database) : Domain.BetRepository
{
    public void Register(Bet bet)
        => database.GetCollection<Bet>("bet")
            .InsertOne(bet);

    public List<Bet> FindByDateRange(long from, long to)
        => database
            .GetCollection<Bet>("bet")
            .Find(c => c.Timestamp >= from && c.Timestamp <= to)
            .ToList();
}