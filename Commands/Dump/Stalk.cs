using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

public class Stalk : BaseCommandModule
{
    /// <summary>
    ///     Where to find the slender file.
    /// </summary>
    private const string Path = "./Resources/Slenders.png";

    /// <summary>
    ///     Slenders dialogue lines.
    /// </summary>
    public static Dictionary<string, string> Lines { get; set; } = null!;

    [Command("stalk")]
    [Aliases("st")]
    [Description("Invoke a discussion between you and one of the five Slenders")]
    public async Task Roast(CommandContext context)
    {
        await context.RespondAsync(new DiscordMessageBuilder().WithFile(new FileStream(Path, FileMode.Open)));
    }

    [Command("stalk")]
    [Description("Invoke a discussion between you and one of the five Slenders")]
    public async Task Discuss(CommandContext context,
        [Description("Slender to talk with")] [RemainingText]
        string name)
    {
        await context.RespondAsync(Lines[name]);
    }
}