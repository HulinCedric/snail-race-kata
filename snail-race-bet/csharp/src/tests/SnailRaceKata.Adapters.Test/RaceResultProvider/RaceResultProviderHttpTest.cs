using SnailRaceKata.Adapters.RaceResultProvider;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class RaceResultProviderHttpTest : RaceResultProviderContractTest, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly Domain.RaceResultProvider _provider;

    private readonly WireMockServerSettings _recordApiSettings = new()
    {
        ProxyAndRecordSettings = new ProxyAndRecordSettings
        {
            Url = "http://localhost:8000",
            SaveMapping = true,
            SaveMappingToFile = true,
            SaveMappingForStatusCodePattern = "2xx",
            ExcludedHeaders = ["Host", "Date"],
        },
        FileSystemHandler = new LocalFileSystemHandler("../../../RaceResultProvider/RaceResultApiSnapshots")
    };
    
    private readonly WireMockServerSettings _playbackApiSettings = new()
    {
        ReadStaticMappings = true,
        FileSystemHandler = new LocalFileSystemHandler("../../../RaceResultProvider/RaceResultApiSnapshots")
    };

    public RaceResultProviderHttpTest()
    {
        // Uncomment the following line to record API responses and refresh the snapshots.
        // var wireMockServer = WireMockServer.Start(_recordApiSettings);
       
        var wireMockServer = WireMockServer.Start(_playbackApiSettings);

        _httpClient = new HttpClient { BaseAddress = new Uri(wireMockServer.Url!) };

        _provider = new RaceResultProviderHttp(_httpClient);
    }

    public void Dispose() => _httpClient.Dispose();

    protected override Domain.RaceResultProvider GetProvider() => _provider;
}