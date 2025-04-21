using SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests.Drivers;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public abstract class BaseSnailRaceApiDriverTest
{
    protected abstract Uri GetBaseUrl();

    protected abstract void SetupSnailRaces(params List<(int number, string name, decimal duration)> snails);

    protected abstract SnailRaceApiDriver CreateSnailRaceApiDriver(HttpClient httpClient);

    [Fact]
    public async Task Should_return_snail_races()
    {
        using var httpClient = new HttpClient();

        httpClient.BaseAddress = GetBaseUrl();

        var snailRaceApiDriver = CreateSnailRaceApiDriver(httpClient);

        // Given
        SetupSnailRaces(
            (number: 1, name: "Fourth", duration: 4.4m),
            (number: 2, name: "Third", duration: 3.3m),
            (number: 3, name: "Second", duration: 2.2m),
            (number: 4, name: "First", duration: 1.1m));

        // When & Then
        await snailRaceApiDriver.ShouldHaveSnailRaces();
    }
}