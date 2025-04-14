using SnailRaceKata.Adapters.Fake;

namespace SnailRaceKata.Test.Adapters.BetRepository;

public class BetRepositoryFakeTest : BetRepositoryContractTest
{
    private readonly BetRepositoryFake _repository = new();

    protected override Domain.BetRepository GetRepository() => _repository;
}