using FluentAssertions;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public abstract class RaceResultProviderContractTest
{
    protected abstract Domain.RaceResultProvider GetProvider();

    [Fact]
    public async Task Provide_race_results()
    {
        var races = await GetProvider().Races();

        races.Races.Should()
            .ContainEquivalentOf(
                new Domain.RaceResultProvider.SnailRace(
                    RaceId: 530572,
                    Timestamp: 1744614752426L,
                    new Domain.RaceResultProvider.Podium(
                        First: new Domain.RaceResultProvider.Snail(2, "Man O'War"),
                        Second: new Domain.RaceResultProvider.Snail(7, "Frankel"),
                        Third: new Domain.RaceResultProvider.Snail(3, "Seabiscuit"))));
    }
}