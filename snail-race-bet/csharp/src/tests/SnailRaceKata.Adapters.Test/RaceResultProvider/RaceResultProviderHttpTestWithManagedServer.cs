using System.Net;
using FluentAssertions;
using SnailRaceKata.Adapters.RaceResultProvider;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderHttpTestWithManagedServer : IDisposable
{
    private readonly HttpClient _httpClient;

    private readonly RaceResultProviderHttp _provider;
    private readonly WireMockServer _wireMockServer;

    public RaceResultProviderHttpTestWithManagedServer()
    {
        _wireMockServer = WireMockServer.Start();

        _httpClient = new HttpClient { BaseAddress = new Uri(_wireMockServer.Url!) };

        _provider = new RaceResultProviderHttp(_httpClient);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _wireMockServer.Dispose();
    }

    [Fact]
    public async Task Podium_should_be_composed_of_three_fastest_ordered_snails()
    {
        _wireMockServer.Given(
                Request.Create()
                    .UsingGet()
                    .WithPath("/results"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(
                        // language=json
                        """
                        {
                          "races": [
                            {
                              "raceId": 641290,
                              "timestamp": 1744654103968,
                              "snails": [
                                { "number": 1, "name": "Fourth", "duration": 4.4 },
                                { "number": 2, "name": "Third",  "duration": 3.3 },
                                { "number": 3, "name": "Second", "duration": 2.2 },
                                { "number": 4, "name": "First",  "duration": 1.1 }
                              ]
                            }
                          ]
                        }
                        """));

        var snailRaces = await _provider.Races();

        snailRaces.Should()
            .BeEquivalentTo(
                new Domain.RaceResultProvider.SnailRaces(
                [
                    new Domain.RaceResultProvider.SnailRace(
                        641290,
                        1744654103968L,
                        new Domain.RaceResultProvider.Podium(
                            new Domain.RaceResultProvider.Snail(4, "First"),
                            new Domain.RaceResultProvider.Snail(3, "Second"),
                            new Domain.RaceResultProvider.Snail(2, "Third")))
                ]));
    }
}