using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Common.Builders;

public class PodiumResultBuilder
{
    private readonly SnailResultBuilder _firstBuilder = new();
    private readonly SnailResultBuilder _secondBuilder = new();
    private readonly SnailResultBuilder _thirdBuilder = new();

    public PodiumResultBuilder OnFirstPlace(int number)
    {
        _firstBuilder.Numbered(number);
        return this;
    }

    public PodiumResultBuilder OnSecondPlace(int number)
    {
        _secondBuilder.Numbered(number);
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