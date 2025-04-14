using FluentAssertions;
using SnailRaceKata.Adapters.Fake;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Domain;

public class BetApplicationTest
{
    private readonly BetApplication _betApplication = new(new BetRepositoryFake(), new RaceResultProviderFake());

    [Fact]
    public void No_winners_when_no_bet_is_placed()
        => _betApplication
            .GetWinnersForLastRace()
            .Should()
            .BeEmpty();

    [Fact]
    public void No_winner_when_bet_is_placed_less_than_3_seconds()
        =>
            // Place a bet through betApplication
            // Configure race result provider simulator to have the corresponding podium but with a timestamp less than 3 seconds
            // Verify there is no winner
            Assert.Fail("Write tests and implement it");

    [Fact]
    public void No_winner_when_the_bet_is_older_than_the_previous_race()
        =>
            // Place a bet through betApplication
            // Configure a race that is older than the bet
            // Configure another race that is newer than the previous race and has a podium that match the pronostic
            // Verify there is no winner
            Assert.Fail("Write tests and implement it");

    public class WinOnlyWhenPronosticExactMatchsPodium
    {
        private readonly BetApplication _betApplication = new BetApplicationTest()._betApplication;

        [Fact]
        public void Exact_match()
            =>
                // Place a bet through betApplication
                // Configure race result provider simulator to have the corresponding podium
                // Verify winners
                Assert.Fail("Write the test and implement it");

        [Fact]
        public void Third_place_differs()
            =>
                // Place a bet through betApplication
                // Configure race result provider simulator to have another podium
                // Verify there is no winner
                Assert.Fail("Write the test and implement it");
    }
}