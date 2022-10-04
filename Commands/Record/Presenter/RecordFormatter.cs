using System;
using System.Data;
using System.Threading.Tasks;
using Bishop.Commands.Record.Model;
using Bishop.Helper;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Presenter;

public class ScoreFormatter
{
    public IKeyBasedCache<ulong, string> Cache { private get; set; } = null!;
    
    // FIXME fix default!
    public async Task<string> Format(ulong userId, CounterCategory category, long score, int? rank = null) =>
        Format(await Cache.GetValue(userId) ?? default!, category, score, rank);

    public string Format(DiscordMember member, CounterCategory category, long score, int? rank = null) =>
         Format(member.Username, category, score, rank);

    private string Format(string username, CounterCategory category, long score, int? rank = null)
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