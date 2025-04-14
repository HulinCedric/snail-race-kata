namespace SnailRaceKata.Domain;

public class BetApplication
{
    private readonly BetRepository _betRepository;
    private readonly RaceResultProvider _raceResultProvider;

    public BetApplication(BetRepository betRepository, RaceResultProvider raceResultProvider)
    {
        _betRepository = betRepository;
        _raceResultProvider = raceResultProvider;
    }

    public void PlaceBet(string gambler, long timestamp, int first, int second, int third)
        => _betRepository.Register(new Bet(gambler, new PodiumPronostic(first, second, third), timestamp));

    public async Task<List<Winner>> GetWinnersForLastRace()
    {
        var bets = _betRepository.FindByDateRange(0, long.MaxValue);
        var races = await _raceResultProvider.Races();

        var lastRace = races.Races.MaxBy(race => race.Timestamp);

        if (lastRace == null)
            return [];

        var betWinners = bets
            .Where(bet => bet.Timestamp > lastRace.Timestamp - 2)
            .Where(
                bet => bet.Pronostic.First == lastRace.Podium.First.Number &&
                       bet.Pronostic.Second == lastRace.Podium.Second.Number &&
                       bet.Pronostic.Third == lastRace.Podium.Third.Number);


        return betWinners.Select(bet => new Winner(bet.Gambler)).ToList();
    }
}