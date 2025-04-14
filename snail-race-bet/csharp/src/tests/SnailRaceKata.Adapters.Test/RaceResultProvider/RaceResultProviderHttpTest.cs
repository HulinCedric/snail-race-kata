using SnailRaceKata.Adapters.RaceResultProvider;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderHttpTest
    : RaceResultProviderContractTest, IDisposable, IClassFixture<SnailRaceServer>
{
    // Set to true to call the Snail Race API and record the responses.
    // Set to false to playback the recorded responses without calling the Snail Race API.
    private const bool IsRecordingModeOn = false;

    private readonly HttpClient _httpClient;

    private readonly Domain.RaceResultProvider _provider;

    public RaceResultProviderHttpTest(SnailRaceServer snailRaceServer)
    {
        var wireMockServer = WireMockServer.Start(Settings(snailRaceServer));

        _httpClient = new HttpClient { BaseAddress = new Uri(wireMockServer.Url!) };

        _provider = new RaceResultProviderHttp(_httpClient);
    }

    private static string ApiSnapshotsPath
        => Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "RaceResultProvider",
            "RaceResultApiSnapshots");

    public void Dispose() => _httpClient.Dispose();

    private static WireMockServerSettings Settings(SnailRaceServer snailRaceServer)
        => IsRecordingModeOn
            ? ProxyAndRecordSettings(snailRaceServer.GetUrl())
            : PlaybackApiSettings();

    private static WireMockServerSettings PlaybackApiSettings()
        => new()
        {
            ReadStaticMappings = true,
            FileSystemHandler = new LocalFileSystemHandler(ApiSnapshotsPath)
        };

    private static WireMockServerSettings ProxyAndRecordSettings(string snailRaceServerUrl)
        => new()
        {
            ProxyAndRecordSettings = new ProxyAndRecordSettings
            {
                Url = snailRaceServerUrl,
                SaveMapping = true,
                SaveMappingToFile = true,
                SaveMappingForStatusCodePattern = "2xx",
                ExcludedHeaders = ["Host"]
            },
            FileSystemHandler = new LocalFileSystemHandler(ApiSnapshotsPath)
        };

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}