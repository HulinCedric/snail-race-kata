using SnailRaceKata.Adapters.BetRepository;

namespace SnailRaceKata.Adapters.Test.BetRepository;

public class BetRepositoryFakeTest : BetRepositoryContractTest
{
    private readonly BetRepositoryFake _repository = new();

    protected override Domain.BetRepository GetRepository() => _repository;
}