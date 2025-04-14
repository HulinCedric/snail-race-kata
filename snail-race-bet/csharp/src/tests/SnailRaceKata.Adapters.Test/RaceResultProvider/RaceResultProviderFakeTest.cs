using SnailRaceKata.Adapters.Fake;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderFakeTest : RaceResultProviderContractTest
{
    private readonly RaceResultProviderFake _provider;

    public RaceResultProviderFakeTest()
    {
        _provider = new RaceResultProviderFake();

        _provider.ThatContains(
            new Domain.RaceResultProvider.SnailRace(
                RaceId: 530572,
                Timestamp: 1744614752426L,
                new Domain.RaceResultProvider.Podium(
                    First: new Domain.RaceResultProvider.Snail(2, "Man O'War"),
                    Second: new Domain.RaceResultProvider.Snail(7, "Frankel"),
                    Third: new Domain.RaceResultProvider.Snail(3, "Seabiscuit"))));
    }

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}