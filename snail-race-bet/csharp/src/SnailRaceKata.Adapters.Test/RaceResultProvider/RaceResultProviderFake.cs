namespace SnailRaceKata.Adapters.Test.RaceResultProvider;

public class RaceResultProviderFake : Domain.RaceResultProvider
{
    public Task<Domain.RaceResultProvider.SnailRaces> Races()
        => Task.FromResult(
            new Domain.RaceResultProvider.SnailRaces(
            [
                new Domain.RaceResultProvider.SnailRace(
                    RaceId: 530572,
                    Timestamp: 1744614752426L,
                    new Domain.RaceResultProvider.Podium(
                        First: new Domain.RaceResultProvider.Snail(2, "Man O'War"),
                        Second: new Domain.RaceResultProvider.Snail(7, "Frankel"),
                        Third: new Domain.RaceResultProvider.Snail(3, "Seabiscuit")))
            ]));
}