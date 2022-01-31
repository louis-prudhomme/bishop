using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provides a command to ease voting between options.
/// </summary>
public class Vote : BaseCommandModule
{
    private const string EMOJI_PREFIX = ":regional_indicator_*:";
    private const char EMOJI_PREFIX_PLACEHOLDER = '*';
    private const string MESSAGE_BASE = "**Aux urnes !**";
    private const int A_ASCII_INDEX = 97;

    /// <summary>
    ///     Valued at 20 as Discord does not allow more reactions to a message.
    /// </summary>
    private const int MAX_POLL_CHOICE = 20;

    [Command("referendum")]
    [Aliases("vote", "v")]
    [Description("Create a poll with the specified options. There must not be more than 20 options.")]
    public async Task Referendum(CommandContext context,
        [Description("Options to choose from")]
        params string[] args)
    {
        switch (args.Length)
        {
            case 1:
                await context.RespondAsync("…Not an easy choice, eh ?");
                break;
            case > MAX_POLL_CHOICE:
                await context.RespondAsync(
                    $"Too much voting options ! Maximum is {MAX_POLL_CHOICE}, got {args.Length}");
                return;
        }

        var messageBuilder = new StringBuilder(MESSAGE_BASE);

        for (var i = 0; i < args.Length; i++)
            messageBuilder.Append($"\n{args[i]} ⇒ {RegionalIndicatorFromIndex(context.Client, i)}");

        var sentMessage = await context.RespondAsync(messageBuilder.ToString());

        for (var i = 0; i < args.Length; i++)
            await sentMessage.CreateReactionAsync(RegionalIndicatorFromIndex(context.Client, i));
    }

    /// <summary>
    ///     Converts an index into a reaction emoji.
    /// </summary>
    /// <param name="client">Discord client.</param>
    /// <param name="index">Index of the reaction emoji.</param>
    /// <returns>Valid Discord emoji.</returns>
    private DiscordEmoji RegionalIndicatorFromIndex(DiscordClient client, int index)
    {
        return DiscordEmoji.FromName(client,
            EMOJI_PREFIX.Replace(EMOJI_PREFIX_PLACEHOLDER, (char) (A_ASCII_INDEX + index)));
    }
}