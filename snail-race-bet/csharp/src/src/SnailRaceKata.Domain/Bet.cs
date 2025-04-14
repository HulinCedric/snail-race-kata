namespace SnailRaceKata.Domain;

public record Bet(string Gambler, PodiumPronostic Pronostic, long Timestamp)
{
    public bool IsInTimeFor(RaceResultProvider.SnailRace race) => Timestamp > race.Timestamp - 2;

    public bool BetIsOn(RaceResultProvider.Podium podium)
        => Pronostic.First == podium.First.Number &&
           Pronostic.Second == podium.Second.Number &&
           Pronostic.Third == podium.Third.Number;
}