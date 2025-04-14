using MongoDB.Driver;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.BetRepository;

public class BetRepositoryMongoDb(IMongoDatabase database) : Domain.BetRepository
{
    private const string BetCollectionName = "bet";

    public async Task Register(Bet bet) => await BetCollection().InsertOneAsync(bet);

    public async Task<List<Bet>> FindByDateRange(long from, long to)
        => await BetCollection()
            .Find(bet => bet.Timestamp >= from && bet.Timestamp <= to)
            .ToListAsync();

    private IMongoCollection<Bet> BetCollection() => database.GetCollection<Bet>(BetCollectionName);
}