using SnailRaceKata.Adapters.Fake;
using static SnailRaceKata.Test.Common.Builders.PodiumResultBuilder;
using static SnailRaceKata.Test.Common.Builders.SnailRaceResultBuilder;
using static SnailRaceKata.Test.Common.Builders.SnailResultBuilder;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderFakeTest : RaceResultProviderContractTest
{
    private readonly RaceResultProviderFake _provider;

    public RaceResultProviderFakeTest()
    {
        _provider = new RaceResultProviderFake();

        _provider.ThatContains(
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

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}