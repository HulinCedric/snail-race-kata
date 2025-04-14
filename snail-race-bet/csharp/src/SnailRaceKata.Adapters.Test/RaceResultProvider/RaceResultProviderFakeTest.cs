namespace SnailRaceKata.Adapters.Test.RaceResultProvider;

public class RaceResultProviderFakeTest : RaceResultProviderContractTest
{
    private readonly Domain.RaceResultProvider _provider = new RaceResultProviderFake();

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}