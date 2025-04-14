using System.Net.Http.Json;

namespace SnailRaceKata.Adapters.RaceResultProvider;

public class RaceResultProviderHttp(HttpClient httpClient) : Domain.RaceResultProvider
{
    private const string ResultsPath = "results";

    public async Task<Domain.RaceResultProvider.SnailRaces> Races() => (await ApiSnailRaces()).ToSnailRaces();

    private async Task<RaceResultProviderHttpRecords.SnailRaces> ApiSnailRaces()
        => (await httpClient.GetFromJsonAsync<RaceResultProviderHttpRecords.SnailRaces>(ResultsPath))!;
}