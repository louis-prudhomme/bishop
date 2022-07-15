using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provides a set of random-based commands.
/// </summary>
public class Randomizer : BaseCommandModule
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

    [Command("Random")]
    [Aliases("rand", "r")]
    [Description("Make a random choice")]
    public async Task RandomChoice(CommandContext context,
        [Description("Options to choose from")]
        params string[] args)
    {
        await context.RespondAsync($"🎲 ⇒ {args[_rand.Next(args.Length)]}");
    }

    [Command("Randomoji")]
    [Aliases("randmoji", "rj")]
    [Description("Picks a random emoji")]
    public async Task RandomEmoji(CommandContext context)
    {
        var emojiCode = BaseEmojiHex + _rand.Next(0, MaxEmojiHex);

        await context.RespondAsync($"I’ve picked : {char.ConvertFromUtf32(emojiCode)}");
    }
}