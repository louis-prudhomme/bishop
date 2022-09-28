using System;
using System.Threading.Tasks;
using Bishop.Config;

namespace Bishop.Commands.Meter;

public class ScoreFormatter
{
    public UserNameCache Cache { private get; set; } = null!;

    public async Task<string> Format(CounterEntity entity, int? rank = null) =>
        await Format(entity.UserId, entity.Category, entity.Score, rank);

    public async Task<string> Format(ulong userId, CounterCategory category, long score, int? rank = null)
    {
        var displayedRank = rank switch
        {
            0 => "🥇 ",
            1 => "🥈 ",
            2 => "🥉 ",
            null => string.Empty,
            _ => "⠀ ⠀"
        };

        var username = await Cache.GetAsync(userId);
        return $"{displayedRank}{username}’s {category} ⇒ {score}";
    }
}