using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send tomatoes to @users.
/// </summary>
public class Tomato : BaseCommandModule
{
    private const string TomatoFilePath = "tomatoes.json";

    private static readonly List<string> Tomatoes = new JsonDeserializer<List<string>>(TomatoFilePath)
        .Get()
        .Result;

    public Random Rand { private get; set; } = null!;


    [Command("tomato")]
    [Aliases("t")]
    [Description("Throw a tomato to @someone")]
    public async Task Throw(CommandContext context,
        [Description("User to throw the tomato at!")]
        DiscordMember member)
    {
        await context.RespondAsync($"{member.Mention} 🍅 ! {Tomatoes[Rand.Next(Tomatoes.Count)]}");
    }
}