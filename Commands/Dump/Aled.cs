using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send help to @users.
/// </summary>
public class Aled : BaseCommandModule
{
    private const string AledFilePath = "aleds.json";

    private static readonly List<string> Aleds = new JsonDeserializer<List<string>>(AledFilePath)
        .Get()
        .Result;

    private readonly Random _rand = new();


    [Command("aled")]
    [Aliases("suicide", "oskour", "halp", "pls")]
    [Description("Help @someone in need")]
    public async Task Help(CommandContext context)
    {
        await context.RespondAsync($"{Aleds[_rand.Next(Aleds.Count)]}");
    }
}