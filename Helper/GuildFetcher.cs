using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Bishop.Helper;

public class GuildFetcher
{
    //TODO Clean this
    public static Func<ulong, Task<DiscordGuild>> FetchAsync { get; set; } = null!;
}