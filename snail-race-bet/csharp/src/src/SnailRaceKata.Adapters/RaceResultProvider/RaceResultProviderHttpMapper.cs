using static SnailRaceKata.Domain.RaceResultProvider;

namespace SnailRaceKata.Adapters.RaceResultProvider;

using Api = RaceResultProviderHttpRecords;

internal static class RaceResultProviderHttpMapper
{
    internal static SnailRaces ToSnailRaces(this Api.SnailRaces apiSnailRaces)
        => new(apiSnailRaces.Races.Select(ToSnailRace).ToList());

    private static SnailRace ToSnailRace(this Api.SnailRace apiSnailRace)
        => new(apiSnailRace.RaceId, apiSnailRace.Timestamp, apiSnailRace.Snails.ToPodium());

    private static Podium ToPodium(this List<Api.Snail> apiSnails)
    {
        var orderedSnails = apiSnails.OrderByDescending(s => s.Duration).ToList();
        return new Podium(
            orderedSnails[0].ToSnail(),
            orderedSnails[1].ToSnail(),
            orderedSnails[2].ToSnail());
    }

    private static Snail ToSnail(this Api.Snail apiSnail)
        => new(apiSnail.Number, apiSnail.Name);
}