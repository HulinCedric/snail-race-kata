using FluentAssertions;
using SnailRaceKata.Adapters.Fake;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Domain;

public class BetApplicationTest
{
    private static readonly RaceResultProvider.SnailRace NineEightSevenPodium = new(
        RaceId: 33,
        Timestamp: 1,
        new RaceResultProvider.Podium(
            First: new RaceResultProvider.Snail(Number: 9, Name: "Turbo"),
            Second: new RaceResultProvider.Snail(Number: 8, Name: "Flash"),
            Third: new RaceResultProvider.Snail(Number: 7, Name: "Speedy")));

    private readonly BetApplication _betApplication;
    private readonly RaceResultProviderFake _raceResultProvider = new();

    public BetApplicationTest() => _betApplication = new BetApplication(new BetRepositoryFake(), _raceResultProvider);

    [Fact]
    public async Task No_winners_when_no_bet_is_placed()
    {
        _raceResultProvider.ThatContains(NineEightSevenPodium);

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }

    [Fact]
    public async Task Winners_when_there_is_an_exact_match()
    {
        _betApplication.PlaceBet("me", 1, 9, 8, 7);

        _raceResultProvider.ThatContains(NineEightSevenPodium);

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEquivalentTo([new Winner("me")]);
    }

    [Fact]
    public async Task No_winners_when_there_is_no_exact_match()
    {
        _betApplication.PlaceBet("me", 1, 9, 8, 2);

        _raceResultProvider.ThatContains(NineEightSevenPodium);

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }

    [Fact]
    public async Task No_winners_when_bet_is_placed_less_than_3_seconds()
    {
        _betApplication.PlaceBet(gambler: "me", timestamp: 1, first: 9, second: 8, third: 7);

        _raceResultProvider.ThatContains(
            new RaceResultProvider.SnailRace(
                RaceId: 33,
                Timestamp: 3,
                new RaceResultProvider.Podium(
                    First: new RaceResultProvider.Snail(Number: 9, Name: "Turbo"),
                    Second: new RaceResultProvider.Snail(Number: 8, Name: "Flash"),
                    Third: new RaceResultProvider.Snail(Number: 7, Name: "Speedy"))));

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }


    [Fact]
    public async Task No_winner_when_the_bet_is_older_than_the_previous_race()
    {
        _betApplication.PlaceBet(gambler: "me", timestamp: 11, first: 9, second: 8, third: 7);

        _raceResultProvider.ThatContains(NineEightSevenPodium);

        _raceResultProvider.ThatContains(
            new RaceResultProvider.SnailRace(
                RaceId: 34,
                Timestamp: 11,
                new RaceResultProvider.Podium(
                    First: new RaceResultProvider.Snail(Number: 7, Name: "Speedy"),
                    Second: new RaceResultProvider.Snail(Number: 8, Name: "Flash"),
                    Third: new RaceResultProvider.Snail(Number: 9, Name: "Turbo"))));

        var winners = await _betApplication.GetWinnersForLastRace();

        winners.Should().BeEmpty();
    }
}