using FluentAssertions;
using SnailRaceKata.Adapters.RaceResultProvider;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderHttpTestWithManagedServer : IDisposable, IClassFixture<ManagedSnailRaceServer>
{
    private readonly HttpClient _httpClient;
    private readonly RaceResultProviderHttp _provider;
    private readonly ManagedSnailRaceServer _snailRaceServer;

    public RaceResultProviderHttpTestWithManagedServer(ManagedSnailRaceServer snailRaceServer)
    {
        _snailRaceServer = snailRaceServer;
        _httpClient = new HttpClient { BaseAddress = new Uri(snailRaceServer.GetUrl()) };
        _provider = new RaceResultProviderHttp(_httpClient);
    }

    public void Dispose()
    {
        _snailRaceServer.Dispose();
        _httpClient.Dispose();
    }

    [Fact]
    public async Task Podium_should_be_composed_of_three_fastest_ordered_snails()
    {
        _snailRaceServer.ReturnARaceWithSnails(
            (number: 1, name: "Fourth", duration: 4.4),
            (number: 2, name: "Third", duration: 3.3),
            (number: 3, name: "Second", duration: 2.2),
            (number: 4, name: "First", duration: 1.1));

        var snailRaces = await _provider.Races();

        snailRaces.Races[0]
            .Podium.Should()
            .BeEquivalentTo(
                new Domain.RaceResultProvider.Podium(
                    new Domain.RaceResultProvider.Snail(4, "First"),
                    new Domain.RaceResultProvider.Snail(3, "Second"),
                    new Domain.RaceResultProvider.Snail(2, "Third")));
    }
}