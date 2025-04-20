namespace SnailRaceKata.Adapters.RaceResultProvider;

public static class RaceResultProviderHttpRecords
{
    public record SnailRaces(List<SnailRace> Races);

    public record SnailRace(int RaceId, long Timestamp, List<Snail> Snails);

    public record Snail(int Number, string Name, float Duration);
}