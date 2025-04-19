using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class ManagedSnailRaceServer : IDisposable
{
    private readonly WireMockServer _wireMockServer = WireMockServer.Start();

    public void Dispose() => _wireMockServer.Dispose();

    public void ReturnARaceWithSnails(params List<(int number, string name, double duration)> snails)
        => _wireMockServer.Given(
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

    private static string Snails(List<(int number, string name, double duration)> snails)
        => $"""
            [
                {string.Join(",", snails.Select(Snail))}
            ]
            """;

    private static string Snail((int number, string name, double duration) snail)
        => FormattableString.Invariant(
            $$"""
              { 
                "number": {{snail.number}},
                "name": "{{snail.name}}",
                "duration": {{snail.duration}}
              }
              """);

    public string GetUrl() => _wireMockServer.Url!;
}