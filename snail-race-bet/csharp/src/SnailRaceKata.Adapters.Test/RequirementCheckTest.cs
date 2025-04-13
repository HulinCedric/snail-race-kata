using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MongoDB.Driver;
using Xunit.Abstractions;

namespace SnailRaceKata.Adapters.Test;

public class RequirementCheckTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Mongo_database_is_reachable()
    {
        using var mongoClient = new MongoClient("mongodb://localhost:27017");

        var databaseNames = mongoClient.ListDatabaseNames().ToList();

        foreach (var databaseName in databaseNames)
        {
            testOutputHelper.WriteLine(databaseName);
        }

        databaseNames.Should().NotBeEmpty();
    }

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