using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Common.Builders;

public class PodiumResultBuilder
{
    private SnailResultBuilder _firstBuilder = new();
    private SnailResultBuilder _secondBuilder = new();
    private SnailResultBuilder _thirdBuilder = new();

    public static PodiumResultBuilder APodium() => new();

    public PodiumResultBuilder OnFirstPlace(SnailResultBuilder snailBuilder)
    {
        _firstBuilder = snailBuilder;
        return this;
    }

    public PodiumResultBuilder OnFirstPlace(int number)
    {
        _firstBuilder.Numbered(number);
        return this;
    }

    public PodiumResultBuilder OnSecondPlace(SnailResultBuilder snailBuilder)
    {
        _secondBuilder = snailBuilder;
        return this;
    }
    
    public PodiumResultBuilder OnSecondPlace(int number)
    {
        _secondBuilder.Numbered(number);
        return this;
    }

    public PodiumResultBuilder OnThirdPlace(SnailResultBuilder snailBuilder)
    {
        _thirdBuilder = snailBuilder;
        return this;
    }
    
    public PodiumResultBuilder OnThirdPlace(int number)
    {
        _thirdBuilder.Numbered(number);
        return this;
    }

    public RaceResultProvider.Podium Build()
        => new(_firstBuilder.Build(), _secondBuilder.Build(), _thirdBuilder.Build());
}