using Bogus;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Common.Builders;

public class SnailRaceResultBuilder
{
    private static readonly Faker Faker = new();

    private PodiumResultBuilder _podiumBuilder = new();

    private int _raceId = Faker.Random.Number(int.MaxValue);
    private long _timestamp = Faker.Date.Recent().Ticks;

    public static SnailRaceResultBuilder ASnailRaceResult() => new();

    public SnailRaceResultBuilder WithRaceId(int raceId)
    {
        _raceId = raceId;
        return this;
    }

    public SnailRaceResultBuilder OnPodium(PodiumResultBuilder podiumBuilder)
    {
        _podiumBuilder = podiumBuilder;
        return this;
    }

    public SnailRaceResultBuilder OnPodium(int first, int second, int third)
    {
        _podiumBuilder.OnFirstPlace(first).OnSecondPlace(second).OnThirdPlace(third);

        return this;
    }

    public SnailRaceResultBuilder AtTimestamp(long timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public RaceResultProvider.SnailRace Build()
        => new(
            _raceId,
            _timestamp,
            _podiumBuilder.Build());

}