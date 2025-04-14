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
                    .WithRaceId(838056)
                    .AtTimestamp(1744652903667L)
                    .OnPodium(
                        APodium()
                            .OnFirstPlace(ASnail().Named("Makybe Diva").Numbered(18))
                            .OnSecondPlace(ASnail().Named("Winx").Numbered(19))
                            .OnThirdPlace(ASnail().Named("Man O'War").Numbered(2)))
                    .Build());
    }
}