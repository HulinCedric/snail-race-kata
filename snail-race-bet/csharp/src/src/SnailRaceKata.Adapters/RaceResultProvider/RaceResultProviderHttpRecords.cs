namespace SnailRaceKata.Adapters.RaceResultProvider;

internal class RaceResultProviderHttpRecords
{
    internal record SnailRaces(List<SnailRace> Races);

    internal record SnailRace(int RaceId, long Timestamp, List<Snail> Snails);

    internal record Snail(int Number, string Name, float Duration);
}