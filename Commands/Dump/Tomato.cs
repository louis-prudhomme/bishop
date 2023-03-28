using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send tomatoes to @users.
/// </summary>
public class Tomato : ApplicationCommandModule
{
    private const string TomatoFilePath = "tomatoes.json";

    private static readonly List<string> Tomatoes = new JsonDeserializer<List<string>>(TomatoFilePath)
        .Get()
        .Result;

    private readonly Random _rand = new();

    [SlashCommand("tomato", "Throw a tomato to @someone")]
    public async Task Throw(InteractionContext context,
        [OptionAttribute("member", "User to throw the tomato at!")]
        DiscordUser member)
    {
        await context.CreateResponseAsync($"{member.Mention} 🍅 ! {Tomatoes[_rand.Next(Tomatoes.Count)]}");
    }
}