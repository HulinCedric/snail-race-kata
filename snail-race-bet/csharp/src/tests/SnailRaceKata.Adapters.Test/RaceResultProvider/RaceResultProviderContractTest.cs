namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public abstract class RaceResultProviderContractTest
{
    private readonly VerifySettings _verifySettings;

    protected RaceResultProviderContractTest()
    {
        _verifySettings = new VerifySettings();
        _verifySettings.UseTypeName(nameof(RaceResultProviderContractTest));
        _verifySettings.DisableRequireUniquePrefix();
    }

    protected abstract Domain.RaceResultProvider GetProvider();

    [Fact]
    public async Task Provide_race_results()
    {
        var races = await GetProvider().Races();
        
        await Verify(races.Races.Take(1), _verifySettings).ScrubMembers("RaceId", "Timestamp", "Name", "Number");
    }
}