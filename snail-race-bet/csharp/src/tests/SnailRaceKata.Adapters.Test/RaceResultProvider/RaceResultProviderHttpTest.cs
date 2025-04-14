using SnailRaceKata.Adapters.RaceResultProvider;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderHttpTest : RaceResultProviderContractTest, IDisposable
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:8000") };
    private readonly Domain.RaceResultProvider _provider;

    public RaceResultProviderHttpTest() => _provider = new RaceResultProviderHttp(_httpClient);

    public void Dispose() => _httpClient.Dispose();

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}