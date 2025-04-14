using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Fake;

public class BetRepositoryFake : Domain.BetRepository
{
    private readonly List<Bet> _bets = [];

    public void Register(Bet bet) => _bets.Add(bet);

    public List<Bet> FindByDateRange(long from, long to)
        => _bets.Where(b => b.Timestamp >= from && b.Timestamp <= to).ToList();
}