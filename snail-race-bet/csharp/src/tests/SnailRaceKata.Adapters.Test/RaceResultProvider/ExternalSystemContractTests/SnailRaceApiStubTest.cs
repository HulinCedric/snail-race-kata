using WireMock.Server;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public class SnailRaceApiStubTest : BaseSnailRaceApiTest, IDisposable
{
    private readonly WireMockServer _apiStub;
    private readonly SnailRaceApiStubDriver _driver;

    public SnailRaceApiStubTest()
    {
        _apiStub = WireMockServer.Start();
        _driver = new SnailRaceApiStubDriver(_apiStub);
    }

    public void Dispose() => _apiStub.Dispose();

    protected override Uri GetBaseUrl() => new(_apiStub.Url!);

    protected override void SetupSnailRaces(params List<(int number, string name, decimal duration)> snails)
        => _driver.WillReturnARaceWithSnails(snails);
}