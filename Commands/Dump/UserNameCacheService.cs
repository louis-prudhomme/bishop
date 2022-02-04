using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Config;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

[Group("cache")]
[RequireOwner]
public class UserNameCacheService : BaseCommandModule
{
    public UserNameCache Cache { private get; set; } = null!;

    [GroupCommand]
    public async Task FetchCache(CommandContext context)
    {
        async Task<(ulong, string)> PairMapper(ulong id) => (id, await AdaptUserIdTo.UserNameAsync(id));

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
        try
        {
            var members = await context.Guild.GetAllMembersAsync();

            var pairs = members
                .Select(member => (member.Id, member.Username));

            foreach (var (item1, item2) in pairs)
                Cache.DirectAdd(item1, item2);

            await context.RespondAsync("Finished");
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }

    [Command("check")]
    public async Task Check(CommandContext context)
    {
        var cache = Cache.Stored;
        string Mapper((ulong, string) tuple) => $"({tuple.Item1}, {tuple.Item2})";

        await context.RespondAsync($"In cache: {cache.Count}");
        await context.RespondAsync($"{cache.Select(Mapper).Aggregate((key1, key2) => string.Join("\n", key1, key2))}");
    }
}