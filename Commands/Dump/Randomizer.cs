using System;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;

using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provides a set of random-based commands.
/// </summary>
public class Randomizer : ApplicationCommandModule
{
    /// <summary>
    ///     Hexadecimal value of the first emoji in UTF-8.
    /// </summary>
    private const int BaseEmojiHex = 0x1F600;

    /// <summary>
    ///     Number of emojis we are interested in UTF-8.
    /// </summary>
    private const int MaxEmojiHex = 44;

    private readonly Random _rand = new();

    [SlashCommand("Random", "Make a random choice")]
    public async Task RandomChoice(InteractionContext context,
        [OptionAttribute("args", "Options to choose from, separated with a space")]
        string args)
    {
        await context.CreateResponseAsync($"🎲 ⇒ {args.Split(' ').Random()}");
    }

    [SlashCommand("Randomoji", "Picks a random emoji")]
    public async Task RandomEmoji(InteractionContext context)
    {
        var emojiCode = BaseEmojiHex + _rand.Next(0, MaxEmojiHex);

        await context.CreateResponseAsync($"I’ve picked : {char.ConvertFromUtf32(emojiCode)}");
    }
}