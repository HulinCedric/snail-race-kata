namespace SnailRaceKata.Domain;

public interface BetRepository
{
    void Register(Bet bet);

    List<Bet> FindByDateRange(long from, long to);
}