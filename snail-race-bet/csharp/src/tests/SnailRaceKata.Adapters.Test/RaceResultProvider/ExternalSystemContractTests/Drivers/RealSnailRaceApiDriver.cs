namespace SnailRaceKata.Test.Adapters.RaceResultProvider.ExternalSystemContractTests.Drivers;

public class RealSnailRaceApiDriver : BaseSnailRaceApiDriver
{
    public RealSnailRaceApiDriver(HttpClient httpClient) : base(httpClient)
    {
    }

    public void CreateSnailRaces(params List<(int number, string name, decimal duration)> snails)
    {
        // No setup possible for real API
    }
}