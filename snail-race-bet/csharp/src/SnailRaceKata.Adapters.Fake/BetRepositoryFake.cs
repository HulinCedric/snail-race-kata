using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Fake;

public class BetRepositoryFake : BetRepository
{
    private readonly List<Bet> _bets = [];

    public Task Register(Bet bet)
    {
        _bets.Add(bet);

        return Task.CompletedTask;
    }

    public Task<List<Bet>> FindByDateRange(long from, long to)
        => Task.FromResult(_bets.Where(b => b.Timestamp >= from && b.Timestamp <= to).ToList());
}