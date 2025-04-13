using FluentAssertions;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Test;

public abstract class BetRepositoryContractTest
{
    protected abstract BetRepository GetRepository();

    [Fact]
    public void Register_a_bet()
    {
        // Given
        var bet = new Bet("gambler", new PodiumPronostic(1, 2, 3), 12345);

        // When
        GetRepository().Register(bet);

        // Then
        GetRepository()
            .FindByDateRange(12345, 12346)
            .Should()
            .BeEquivalentTo([bet]);
    }

    [Fact]
    public void Retrieve_only_bets_inside_the_time_range()
    {
        // Given
        var outerLowerRange = new Bet("gambler1", new PodiumPronostic(1, 2, 3), 12345);
        var equalToLowerRange = new Bet("gambler2", new PodiumPronostic(1, 2, 3), 12346);
        var inRange = new Bet("gambler3", new PodiumPronostic(1, 2, 3), 12347);
        var equalToUpperRange = new Bet("gambler4", new PodiumPronostic(1, 2, 3), 12348);
        var outerUpperRange = new Bet("gambler5", new PodiumPronostic(1, 2, 3), 12349);

        GetRepository().Register(outerLowerRange);
        GetRepository().Register(equalToLowerRange);
        GetRepository().Register(inRange);
        GetRepository().Register(equalToUpperRange);
        GetRepository().Register(outerUpperRange);

        // When
        var bets = GetRepository().FindByDateRange(12346, 12348);

        // Then
        bets.Should().BeEquivalentTo([equalToLowerRange, inRange, equalToUpperRange]);
    }
}