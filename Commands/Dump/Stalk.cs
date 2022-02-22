using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

public class Stalk : BaseCommandModule
{
    private const string StalkFilePath = "slenders.json";

    /// <summary>
    ///     Slenders dialogue lines.
    /// </summary>
    private static readonly Dictionary<string, string> Lines =
        new JsonDeserializer<Dictionary<string, string>>(StalkFilePath)
            .Get()
            .Result;

    [Command("stalk")]
    [Description("Invoke a discussion between you and one of the five Slenders")]
    public async Task Discuss(CommandContext context,
        [Description("Slender to talk with")] [RemainingText]
        string name)
    {
        await context.RespondAsync(Lines[name]);
    }
}