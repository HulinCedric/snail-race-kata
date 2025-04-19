using FluentAssertions;
using SnailRaceKata.Adapters.RaceResultProvider;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

/// <summary>
///     Integration test with a stub server.
///     The test fully controls the stub server.
///     The stub server encapsulates the contracts discovered during real interaction
///     or can simulate contracts established between teams or written in documentation.
///     This allows testing the integration with technology with a sociable approach (http, mapper, etc.).
/// </summary>
public class RaceResultProviderHttpTestWithStubServer :
    RaceResultProviderContractTest, IDisposable, IClassFixture<SnailRaceWireMockServer>
{
    private readonly HttpClient _httpClient;
    private readonly RaceResultProviderHttp _provider;

    public RaceResultProviderHttpTestWithStubServer(SnailRaceWireMockServer snailRaceServer)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(snailRaceServer.GetUrl()) };
        _provider = new RaceResultProviderHttp(_httpClient);

        snailRaceServer.ReturnARaceWithSnails(
            (number: 1, name: "Fourth", duration: 4.4),
            (number: 2, name: "Third", duration: 3.3),
            (number: 3, name: "Second", duration: 2.2),
            (number: 4, name: "First", duration: 1.1));
    }

    public void Dispose() => _httpClient.Dispose();

    [Fact]
    public async Task Podium_should_be_composed_of_three_fastest_ordered_snails()
    {
        var snailRaces = await _provider.Races();

        snailRaces.Races[0]
            .Podium.Should()
            .BeEquivalentTo(
                new Domain.RaceResultProvider.Podium(
                    new Domain.RaceResultProvider.Snail(4, "First"),
                    new Domain.RaceResultProvider.Snail(3, "Second"),
                    new Domain.RaceResultProvider.Snail(2, "Third")));
    }

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}