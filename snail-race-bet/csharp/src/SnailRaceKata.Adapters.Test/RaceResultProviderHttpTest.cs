using FluentAssertions;
using SnailRaceKata.Adapters.RaceResultProvider;

namespace SnailRaceKata.Adapters.Test;

public class RaceResultProviderHttpTest
{
    [Fact]
    public async Task Provide_race_results()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8000") };

        var provider = new RaceResultProviderHttp(httpClient);

        var races = await provider.Races();

        races.Races.Should().NotBeEmpty();
    }
}