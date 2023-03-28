using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;


using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

public class Stalk : ApplicationCommandModule
{
    private const string StalkFilePath = "slenders.json";

    /// <summary>
    ///     Slenders dialogue lines.
    /// </summary>
    private static readonly Dictionary<string, string> Lines =
        new JsonDeserializer<Dictionary<string, string>>(StalkFilePath)
            .Get()
            .Result;

    [SlashCommand("stalk", "Invoke a discussion between you and one of the five Slenders")]
    public async Task Discuss(InteractionContext context,
        [OptionAttribute("Name", "Slender to talk with")]
        string name)
    {
        await context.CreateResponseAsync(Lines[name]);
    }
}