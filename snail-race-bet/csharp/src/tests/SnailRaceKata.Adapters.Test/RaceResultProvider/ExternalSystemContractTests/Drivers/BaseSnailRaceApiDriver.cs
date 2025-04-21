using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SnailRaceKata.Adapters.RaceResultProvider;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests.Drivers;

public abstract class BaseSnailRaceApiDriver : SnailRaceApiDriver
{
    private readonly HttpClient _httpClient;

    protected BaseSnailRaceApiDriver(HttpClient httpClient) => _httpClient = httpClient;

    public async Task ShouldHaveSnailRaces()
    {
        var response = await _httpClient.GetAsync("/results");

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