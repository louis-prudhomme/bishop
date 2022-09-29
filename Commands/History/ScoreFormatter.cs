using System.Threading.Tasks;
using Bishop.Config;
using DSharpPlus.Entities;

namespace Bishop.Commands.History;

public class ScoreFormatter
{
    public UserNameCache Cache { private get; set; } = null!;


    public async Task<string> Format(ulong userId, CounterCategory category, long score, int? rank = null) =>
        Format(await Cache.GetAsync(userId), category, score, rank);

    public string Format(DiscordMember member, CounterCategory category, long score, int? rank = null) =>
         Format(member.Username, category, score, rank);

    public string Format(string username, CounterCategory category, long score, int? rank = null)
    {
        var displayedRank = rank switch
        {
            0 => "🥇 ",
            1 => "🥈 ",
            2 => "🥉 ",
            null => string.Empty,
            _ => "⠀ ⠀"
        };
        
        return $"{displayedRank}{username}’s {category} ⇒ {score}";
    }
}