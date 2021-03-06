using System.Linq;
using System.Threading.Tasks;
using Bishop.Config;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

[Group("cache")]
[RequireOwner]
public class UserNameCacheService : BaseCommandModule
{
    public UserNameCache Cache { private get; set; } = null!;

    [GroupCommand]
    public async Task FetchCache(CommandContext context)
    {
        async Task<(ulong, string)> PairMapper(ulong id)
        {
            return (id, await AdaptUserIdTo.UserNameAsync(id));
        }

        var pairs = await Task.WhenAll(context.Guild.Members
            .Select(pair => pair.Key)
            .Select(PairMapper)
            .ToList());

        foreach (var (item1, item2) in pairs)
            Cache.DirectAdd(item1, item2);

        await context.RespondAsync("Finished");
    }

    [Command("force")]
    public async Task ForceFetchCache(CommandContext context)
    {
        var members = await context.Guild.GetAllMembersAsync();

        var pairs = members
            .Select(member => (member.Id, member.Username));

        foreach (var (item1, item2) in pairs)
            Cache.DirectAdd(item1, item2);

        await context.RespondAsync("Finished");
    }

    [Command("check")]
    public async Task Check(CommandContext context)
    {
        var cache = Cache.Stored;

        string Mapper((ulong, string) tuple)
        {
            return $"({tuple.Item1}, {tuple.Item2})";
        }

        await context.RespondAsync($"In cache: {cache.Count}");
        await context.RespondAsync($"{cache.Select(Mapper).JoinWithNewlines()}");
    }

    [Command("clear")]
    public void Clear(CommandContext context)
    {
        Cache.Nuke();
    }
}