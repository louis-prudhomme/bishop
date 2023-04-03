using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;
using Bishop.Helper.Extensions;

using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send help to @users.
/// </summary>
public class Aled : ApplicationCommandModule
{
    private const string AledFilePath = "aleds.json";

    private static readonly List<string> Aleds = new JsonDeserializer<List<string>>(AledFilePath)
        .Get()
        .Result;

    private readonly Random _rand = new();

    [SlashCommand("aled", "Help someone in need")]
    public async Task Help(InteractionContext context)
    {
        await context.CreateResponseAsync($"{Aleds.Random()}");
    }
}