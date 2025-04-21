using SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests.Drivers;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public class RealSnailRaceApiDriverTest : BaseSnailRaceApiDriverTest, IClassFixture<SnailRaceContainerServer>
{
    private readonly SnailRaceContainerServer _snailRaceContainerServer;

    public RealSnailRaceApiDriverTest(SnailRaceContainerServer snailRaceContainerServer)
        => _snailRaceContainerServer = snailRaceContainerServer;

    protected sealed override Uri GetBaseUrl() => _snailRaceContainerServer.GetUrl();

    protected override void SetupSnailRaces(params List<(int number, string name, decimal duration)> snails)
    {
        var driver =  new RealSnailRaceApiDriver(new HttpClient() { BaseAddress = GetBaseUrl() });
        driver.CreateSnailRaces(snails);
    }

    protected sealed override SnailRaceApiDriver CreateSnailRaceApiDriver(HttpClient httpClient)
        => new RealSnailRaceApiDriver(httpClient);
}