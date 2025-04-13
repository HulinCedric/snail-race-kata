using MongoDB.Driver;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters;

public class BetRepositoryMongoDb : BetRepository
{
    public BetRepositoryMongoDb(IMongoDatabase database)
    {
    }

    public void Register(Bet bet)
    {
    }

    public List<Bet> FindByDateRange(long from, long to)
    {
        return null;
    }
}