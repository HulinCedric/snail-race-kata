using SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests.Drivers;
using WireMock.Server;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public class SnailRaceApiStubDriverTest : BaseSnailRaceApiDriverTest, IDisposable
{
    private readonly WireMockServer _apiStub;
    private readonly SnailRaceApiStubDriver _driver;

    public SnailRaceApiStubDriverTest()
    {
        _apiStub = WireMockServer.Start();
        _driver = new SnailRaceApiStubDriver(_apiStub);
    }

    public void Dispose() => _apiStub.Dispose();

    protected override Uri GetBaseUrl() => new(_apiStub.Url!);

    protected override void SetupSnailRaces(params List<(int number, string name, decimal duration)> snails)
        => _driver.WillReturnARaceWithSnails(snails);

    protected override SnailRaceApiDriver CreateSnailRaceApiDriver(HttpClient httpClient)
        => new SnailRaceApiStubDriver(httpClient, _apiStub);
}