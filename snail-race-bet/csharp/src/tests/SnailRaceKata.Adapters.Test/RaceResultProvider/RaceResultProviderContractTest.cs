using FluentAssertions;
using static SnailRaceKata.Test.Common.Builders.PodiumResultBuilder;
using static SnailRaceKata.Test.Common.Builders.SnailRaceResultBuilder;
using static SnailRaceKata.Test.Common.Builders.SnailResultBuilder;

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
                ASnailRaceResult()
                    .WithRaceId(530572)
                    .AtTimestamp(1744614752426L)
                    .OnPodium(
                        APodium()
                            .OnFirstPlace(ASnail().Named("Man O'War").Numbered(2))
                            .OnSecondPlace(ASnail().Named("Frankel").Numbered(7))
                            .OnThirdPlace(ASnail().Named("Seabiscuit").Numbered(3)))
                    .Build());
    }
}