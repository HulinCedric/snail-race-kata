using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Fake;

public class RaceResultProviderFake : RaceResultProvider
{
    private readonly List<RaceResultProvider.SnailRace> _snailRaces = [];

    public Task<RaceResultProvider.SnailRaces> Races()
        => Task.FromResult(new RaceResultProvider.SnailRaces(_snailRaces));

    public void ThatContains(RaceResultProvider.SnailRace snailRace) => _snailRaces.Add(snailRace);
}