using SnailRaceKata.Domain;

namespace SnailRaceKata.Adapters.Test;

public class BetRepositoryFakeTest : BetRepositoryContractTest
{
    private readonly BetRepositoryFake _repository = new();

    protected override BetRepository GetRepository() => _repository;
}