namespace SnailRaceKata.Domain;

public class BetApplication
{
    private BetApplication(BetRepository betRepository, RaceResultProvider raceResultProvider)
    {
    }

    private void PlaceBet(string gambler, long timestamp, int first, int second, int third)
    {
    }

    private List<Winner> GetWinnersForLastRace() => null;
}