namespace SnailRaceKata.Domain;

public class BetApplication
{
    private static readonly List<Winner> NoWinners = [];

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
        var bets = GetAllBets();
        var lastRace = await GetLastRace();
        if (lastRace is null) return NoWinners;

        var winningBets = FindExactMatchBets(bets, lastRace);

        return ToWinners(winningBets);
    }

    private async Task<RaceResultProvider.SnailRace?> GetLastRace()
    {
        var races = await _raceResultProvider.Races();

        return races.LastRace();
    }

    private List<Bet> GetAllBets() => _betRepository.FindByDateRange(0, long.MaxValue);

    private static IEnumerable<Bet> FindExactMatchBets(List<Bet> bets, RaceResultProvider.SnailRace race)
        => bets
            .Where(bet => bet.IsInTimeFor(race))
            .Where(bet => bet.BetIsOn(race.Podium));

    private static List<Winner> ToWinners(IEnumerable<Bet> winningBets)
        => winningBets.Select(bet => new Winner(bet.Gambler)).ToList();
}