namespace SnailRaceKata.Domain;

public interface RaceResultProvider
{
    SnailRaces Races();

    record Snail(int Number, string Name);

    record Podium(Snail First, Snail Second, Snail Third);

    record SnailRace(int RaceId, long Timestamp, Podium Podium);

    record SnailRaces(List<SnailRace> Races);
}