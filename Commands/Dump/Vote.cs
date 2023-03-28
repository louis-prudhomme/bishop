using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

//FIXME: i do not work as of right now
/// <summary>
///     Provides a command to ease voting between options.
/// </summary>
public class Vote : ApplicationCommandModule
{
    private const string EmojiPrefix = ":regional_indicator_*:";
    private const char EmojiPrefixPlaceholder = '*';
    private const string MessageBase = "**Aux urnes !**";
    private const int AAsciiIndex = 97;

    /// <summary>
    ///     Valued at 20 as Discord does not allow more reactions to a message.
    /// </summary>
    private const int MaxPollChoice = 20;

    [SlashCommand("referendum", "Create a poll with the specified options. There must not be more than 20 options.")]
    public async Task Referendum(InteractionContext context,
        [OptionAttribute("args", "Options to choose from")]
        string args) // FIXME: not very practical
    {
        switch (args.Split(' ').Length)
        {
            case 1:
                await context.CreateResponseAsync("…Not an easy choice, eh ?");
                break;
            case > MaxPollChoice:
                await context.CreateResponseAsync(
                    $"Too much voting options ! Maximum is {MaxPollChoice}, got {args.Length}");
                return;
        }

        var contentBuilder = new StringBuilder();
        for (var i = 0; i < args.Length; i++)
            contentBuilder.Append($"\n{args[i]} ⇒ {RegionalIndicatorFromIndex(context.Client, i)}");

        var builder = new DiscordInteractionResponseBuilder
        {
            Content = MessageBase,
        };

        var buttons = args.Split(' ').Select((arg, i) => (Option: arg, Emoji: RegionalIndicatorFromIndex(context.Client, i)))
                .Select(tuple => (tuple.Option, Emoji: new DiscordComponentEmoji(tuple.Emoji)))
                .Select(tuple => new DiscordButtonComponent(ButtonStyle.Primary, tuple.Option, string.Empty, false, tuple.Emoji))
                .Chunk(5)
            .Select(buttons => new DiscordActionRowComponent(buttons));
        builder.AddComponents(buttons);
        await context.CreateResponseAsync(builder);
    }

    /// <summary>
    ///     Converts an index into a reaction emoji.
    /// </summary>
    /// <param name="client">Discord client.</param>
    /// <param name="index">Index of the reaction emoji.</param>
    /// <returns>Valid Discord emoji.</returns>
    private string RegionalIndicatorFromIndex(DiscordClient client, int index)
    {
        return EmojiPrefix.Replace(EmojiPrefixPlaceholder, (char) (AAsciiIndex + index));
    }
}