using Bogus;
using SnailRaceKata.Domain;

namespace SnailRaceKata.Test.Common.Builders;

public class SnailResultBuilder
{
    private static readonly Faker Faker = new();

    private string _name = Faker.Name.FirstName();
    private int _number = Faker.Random.Number(int.MaxValue);

    public static SnailResultBuilder ASnail() => new();

    public SnailResultBuilder Numbered(int number)
    {
        _number = number;
        return this;
    }

    public SnailResultBuilder Named(string name)
    {
        _name = name;
        return this;
    }

    public RaceResultProvider.Snail Build() => new(_number, _name);
}