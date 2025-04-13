using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Test;

public class BetBuilder
{
    private string _gambler = "gambler";
    private PodiumPronostic _podium = new(1, 2, 3);
    private long _timestamp = 12345;

    public static BetBuilder ABet() => new();

    public BetBuilder WithGambler(string gambler)
    {
        _gambler = gambler;
        return this;
    }

    public BetBuilder WithPodium(PodiumPronostic podium)
    {
        _podium = podium;
        return this;
    }

    public BetBuilder WithTimestamp(long timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public Bet Build() => new(_gambler, _podium, _timestamp);
}