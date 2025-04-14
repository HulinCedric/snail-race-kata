namespace SnailRaceKata.Domain;

public class BetApplication
{
    public BetApplication(BetRepository betRepository, RaceResultProvider raceResultProvider)
    {
    }

    private void PlaceBet(string gambler, long timestamp, int first, int second, int third)
    {
    }

    public List<Winner> GetWinnersForLastRace() => null;
}