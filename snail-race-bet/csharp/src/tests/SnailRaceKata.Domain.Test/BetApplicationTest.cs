using FluentAssertions;
using SnailRaceKata.Adapters.Fake;
using SnailRaceKata.Domain;
using static SnailRaceKata.Test.Common.Builders.SnailRaceResultBuilder;

namespace SnailRaceKata.Test.Domain;

public class BetApplicationTest
{
    private readonly BetApplication _betApplication;
    private readonly RaceResultProviderFake _raceResultProvider = new();

    public BetApplicationTest() => _betApplication = new BetApplication(new BetRepositoryFake(), _raceResultProvider);

    [Fact]
    public async Task No_winners_when_no_bet_is_placed()
    {
        _raceResultProvider.ThatContains(ASnailRaceResult().Build());

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }

    [Fact]
    public async Task Winners_when_there_is_an_exact_match()
    {
        await _betApplication.PlaceBet(gambler: "me", timestamp: 1, first: 9, second: 8, third: 7);

        _raceResultProvider.ThatContains(
            ASnailRaceResult()
                .AtTimestamp(1)
                .OnPodium(first: 9, second: 8, third: 7)
                .Build());

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEquivalentTo([new Winner("me")]);
    }

    [Fact]
    public async Task No_winners_when_there_is_no_exact_match()
    {
        await _betApplication.PlaceBet(gambler: "me", timestamp: 1, first: 9, second: 8, third: 2);

        _raceResultProvider.ThatContains(
            ASnailRaceResult()
                .AtTimestamp(1)
                .OnPodium(first: 9, second: 8, third: 7)
                .Build());

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }

    [Fact]
    public async Task No_winners_when_bet_is_placed_less_than_3_seconds()
    {
        await _betApplication.PlaceBet(gambler: "me", timestamp: 1, first: 9, second: 8, third: 7);

        _raceResultProvider.ThatContains(
            ASnailRaceResult()
                .AtTimestamp(3)
                .OnPodium(first: 9, second: 8, third: 7)
                .Build());

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }


    [Fact]
    public async Task No_winner_when_the_bet_is_older_than_the_previous_race()
    {
        await _betApplication.PlaceBet(gambler: "me", timestamp: 11, first: 9, second: 8, third: 7);

        _raceResultProvider.ThatContains(
            ASnailRaceResult()
                .AtTimestamp(1)
                .OnPodium(first: 9, second: 8, third: 7)
                .Build());

        _raceResultProvider.ThatContains(
            ASnailRaceResult()
                .AtTimestamp(11)
                .OnPodium(first: 7, second: 8, third: 9)
                .Build());

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }
}