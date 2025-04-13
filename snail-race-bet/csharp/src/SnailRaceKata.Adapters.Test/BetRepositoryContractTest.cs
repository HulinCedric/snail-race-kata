using FluentAssertions;
using SnailRaceKata.Domain;
using static SnailRaceKata.Adapters.Test.BetBuilder;

namespace SnailRaceKata.Adapters.Test;

public abstract class BetRepositoryContractTest
{
    protected abstract BetRepository GetRepository();

    [Fact]
    public void Register_a_bet()
    {
        // Given
        var bet = ABet().WithTimestamp(12345).Build();

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
        var outerLowerRange = ABet().WithTimestamp(12345).Build();
        var equalToLowerRange = ABet().WithTimestamp(12346).Build();
        var inRange = ABet().WithTimestamp(12347).Build();
        var equalToUpperRange = ABet().WithTimestamp(12348).Build();
        var outerUpperRange = ABet().WithTimestamp(12349).Build();

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