using FluentAssertions;
using static SnailRaceKata.Test.Common.Builders.BetBuilder;

namespace SnailRaceKata.Test.Adapters.BetRepository;

public abstract class BetRepositoryContractTest
{
    protected abstract Domain.BetRepository GetRepository();

    [Fact]
    public async Task Register_a_bet()
    {
        // Given
        var bet = ABet().WithTimestamp(12345).Build();

        // When
        await GetRepository().Register(bet);

        // Then
        (await GetRepository().FindByDateRange(12345, 12346))
            .Should()
            .BeEquivalentTo([bet]);
    }

    [Fact]
    public async Task Retrieve_only_bets_inside_the_time_range()
    {
        // Given
        var outerLowerRange = ABet().WithTimestamp(12345).Build();
        var equalToLowerRange = ABet().WithTimestamp(12346).Build();
        var inRange = ABet().WithTimestamp(12347).Build();
        var equalToUpperRange = ABet().WithTimestamp(12348).Build();
        var outerUpperRange = ABet().WithTimestamp(12349).Build();

        await GetRepository().Register(outerLowerRange);
        await GetRepository().Register(equalToLowerRange);
        await GetRepository().Register(inRange);
        await GetRepository().Register(equalToUpperRange);
        await GetRepository().Register(outerUpperRange);

        // When
        var bets = await GetRepository().FindByDateRange(12346, 12348);

        // Then
        bets.Should().BeEquivalentTo([equalToLowerRange, inRange, equalToUpperRange]);
    }
}