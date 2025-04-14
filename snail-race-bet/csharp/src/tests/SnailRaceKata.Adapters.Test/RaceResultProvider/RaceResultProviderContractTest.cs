using FluentAssertions;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public abstract class RaceResultProviderContractTest
{
    protected abstract Domain.RaceResultProvider GetProvider();

    [Fact]
    public async Task Provide_race_results()
    {
        var races = await GetProvider().Races();

        races.Races.Should().NotBeEmpty();
    }
}