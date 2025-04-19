using SnailRaceKata.Adapters.RaceResultProvider;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

/// <summary>
///     Contract test with real server.
///     Use a record and playback approach:
///     - record the API responses in the discovery phase
///     - play recorded API responses back in the test phase
///     This allows to test the integration with the real server without calling it every time.
///     Useful when the server is slow, unreliable, rate limited or has a cost.
///     The recorded API responses can be refreshed with a recurring strategy.
/// </summary>
/// <remarks>
///     For developer experience,
///     the test use a <see cref="SnailRaceContainerServer" />
///     to remove requirement of launching test instance manually.
///     In real life, we do not have a container for a real API dependency.
/// </remarks>
public class RaceResultProviderHttpTestWithRealServer
    : RaceResultProviderContractTest, IDisposable, IClassFixture<SnailRaceContainerServer>
{
    // Set to true to call the Snail Race API and record the responses.
    // Set to false to playback the recorded responses without calling the Snail Race API.
    private const bool IsRecordingModeOn = false;

    private readonly HttpClient _httpClient;

    private readonly Domain.RaceResultProvider _provider;

    public RaceResultProviderHttpTestWithRealServer(SnailRaceContainerServer snailRaceServer)
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

    private static WireMockServerSettings Settings(SnailRaceContainerServer snailRaceServer)
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