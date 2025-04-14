using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MongoDB.Driver;

namespace SnailRaceKata.Test.Adapters;

public class RequirementCheckTest
{
    [Fact]
    public async Task Race_result_server_is_accessible()
    {
        using var httpClient = new HttpClient();

        var response = await httpClient.GetAsync("http://localhost:8000/results");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        var snailRacesResults = await response.Content.ReadFromJsonAsync<SnailRacesResults>();
        snailRacesResults!.Races.Should().NotBeEmpty();
    }

    private record SnailRacesResults(List<SnailRaceResult> Races);

    private record SnailRaceResult(int RaceId, long Timestamp, List<Snail> Snails);

    private record Snail(int Number, string Name, float Duration);
}