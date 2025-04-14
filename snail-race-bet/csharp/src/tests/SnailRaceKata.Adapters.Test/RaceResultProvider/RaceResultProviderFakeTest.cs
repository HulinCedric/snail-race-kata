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
                .WithRaceId(530572)
                .AtTimestamp(1744614752426L)
                .OnPodium(
                    APodium()
                        .OnFirstPlace(ASnail().Named("Man O'War").Numbered(2))
                        .OnSecondPlace(ASnail().Named("Frankel").Numbered(7))
                        .OnThirdPlace(ASnail().Named("Seabiscuit").Numbered(3)))
                .Build());
    }

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}