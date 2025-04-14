using Bogus;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Common.Builders;

public class SnailRaceResultBuilder
{
    private static readonly Faker Faker = new();

    private readonly PodiumResultBuilder _podiumBuilder = new();
   
    private readonly int _raceId = Faker.Random.Number(int.MaxValue);
    private long _timestamp = Faker.Date.Recent().Ticks;

    public static SnailRaceResultBuilder ASnailRaceResult() => new();

    public SnailRaceResultBuilder OnPodium(int first, int second, int third)
    {
        _podiumBuilder.OnFirstPlace(first).OnSecondPlace(second).OnThirdPlace(third);

        return this;
    }

    public SnailRaceResultBuilder AtTimestamp(int timestamp)
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