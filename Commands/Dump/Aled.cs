using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send help to @users.
/// </summary>
public class Aled : BaseCommandModule
{
    public Random Rand { private get; set; } = null!;
    public static List<string> Aleds { get; set; } = null!;

    [Command("aled")]
    [Aliases("suicide", "oskour", "halp", "pls")]
    [Description("Help @someone in need")]
    public async Task Help(CommandContext context)
    {
        await context.RespondAsync($"{Aleds[Rand.Next(Aleds.Count)]}");
    }
}