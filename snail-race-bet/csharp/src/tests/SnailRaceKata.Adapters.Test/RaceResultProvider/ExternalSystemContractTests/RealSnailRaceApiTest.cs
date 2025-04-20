namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public class RealSnailRaceApiTest : BaseSnailRaceApiTest, IClassFixture<SnailRaceContainerServer>
{
    private readonly SnailRaceContainerServer _snailRaceContainerServer;

    public RealSnailRaceApiTest(SnailRaceContainerServer snailRaceContainerServer)
        => _snailRaceContainerServer = snailRaceContainerServer;

    protected override Uri GetBaseUrl() => _snailRaceContainerServer.GetUrl();

    protected override void SetupSnailRaces(params List<(int number, string name, decimal duration)> snails)
    {
    }
}