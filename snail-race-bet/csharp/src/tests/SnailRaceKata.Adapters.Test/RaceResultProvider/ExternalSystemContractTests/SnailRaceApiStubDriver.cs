using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests;

public class SnailRaceApiStubDriver
{
    private readonly WireMockServer _apiStub;

    public SnailRaceApiStubDriver(WireMockServer apiStub) => _apiStub = apiStub;

    public void WillReturnARaceWithSnails(params List<(int number, string name, decimal duration)> snails)
        => _apiStub.Given(
                Request.Create()
                    .UsingGet()
                    .WithPath("/results"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(
                        // language=json
                        $$"""
                          {
                            "races": [
                              {
                                "raceId": 641290,
                                "timestamp": 1744654103968,
                                "snails": {{Snails(snails)}}
                              }
                            ]
                          }
                          """));

    private static string Snails(List<(int number, string name, decimal duration)> snails)
        => $"""
            [
                {string.Join(",", snails.Select(Snail))}
            ]
            """;

    private static string Snail((int number, string name, decimal duration) snail)
        => FormattableString.Invariant(
            $$"""
              { 
                "number": {{snail.number}},
                "name": "{{snail.name}}",
                "duration": {{snail.duration}}
              }
              """);
}