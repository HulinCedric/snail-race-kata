namespace SnailRaceKata.Domain;

public interface BetRepository
{
    Task Register(Bet bet);

    Task<List<Bet>> FindByDateRange(long from, long to);
}