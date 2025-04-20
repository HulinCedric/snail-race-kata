using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SnailRaceKata.Adapters.RaceResultProvider;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public abstract class BaseSnailRaceApiTest
{
    protected abstract Uri GetBaseUrl();

    protected abstract void SetupSnailRaces(params List<(int number, string name, decimal duration)> snails);

    [Fact]
    public async Task Should_return_snail_races()
    {
        using var httpClient = new HttpClient();

        httpClient.BaseAddress = GetBaseUrl();

        SetupSnailRaces(
            (number: 1, name: "Fourth", duration: 4.4m),
            (number: 2, name: "Third", duration: 3.3m),
            (number: 3, name: "Second", duration: 2.2m),
            (number: 4, name: "First", duration: 1.1m));

        var response = await httpClient.GetAsync("/results");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiSnailRaces = await response.Content.ReadFromJsonAsync<RaceResultProviderHttpRecords.SnailRaces>();

        apiSnailRaces!.Races.Should()
            .AllSatisfy(race =>
            {
                race.Snails.Should()
                    .HaveCountGreaterOrEqualTo(3)
                    .And
                    .OnlyHaveUniqueItems(snail => snail.Number)
                    .And
                    .OnlyHaveUniqueItems(snail => snail.Name);
            });
    }
}