using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provides a command to ease voting between options.
/// </summary>
public class Referendum : ApplicationCommandModule
{
    private const string EmojiPrefix = ":regional_indicator_*:";
    private const char EmojiPrefixPlaceholder = '*';
    private const string MessageBase = "**Aux urnes !**";
    private const int AAsciiIndex = 97;

    /// <summary>
    ///     Valued at 20 as Discord does not allow more reactions to a message.
    /// </summary>
    private const int MaxPollChoice = 20;

    public VoteAnswerEventHandler Booth { get; set; } = null!;

    [SlashCommand("vote", "Create a poll with the specified options. There must not be more than 20 options.")]
    public async Task VoteDefault(InteractionContext context,
        [OptionAttribute("options", "Options to choose from, separated with spaces")]
        string args)
    {
        await Vote(context, args, 1);
    }

    [SlashCommand("votewhile", "Create a poll with the specified options. There must not be more than 20 options.")]
    public async Task Vote(InteractionContext context,
        [OptionAttribute("options", "Options to choose from, separated with spaces")]
        string args,
        [Maximum(10)] [Minimum(1)] [OptionAttribute("while", "Time for the vote, in minutes (< 10)")]
        long during)
    {
        try
        {
            var options = args
                .SplitArguments()
                .Select(option => option.Trim())
                .Select((option, i) => new VoteOption(option, Guid.NewGuid().ToString(), RegionalIndicatorFromIndex(i)))
                .ToList();

            switch (options.Count)
            {
                case 1:
                    await context.CreateResponseAsync("…Not an easy choice, eh ?");
                    return;
                case > MaxPollChoice:
                    await context.CreateResponseAsync($"Too much voting options ! Maximum is {MaxPollChoice}, got {options.Count}");
                    return;
            }

            var labels = options
                .Select(option => $"{option.EmojiName} => {option.Label}");
            var buttons = options
                .Select(option => (Option: option, Emoji: DiscordComponentEmojiFromIndex(option.EmojiName, context)))
                .Select(tuple => CreateDiscordButton(tuple.Option, tuple.Emoji))
                .Chunk(5)
                .Select(buttons => new DiscordActionRowComponent(buttons));

            var announcement = new DiscordInteractionResponseBuilder
            {
                Title = MessageBase,
                Content = MessageBase + "\n" + labels.JoinWithNewlines()
            };
            announcement.AddComponents(buttons);

            options.ForEach(option => Booth.Register(option.Id));
            await context.CreateResponseAsync(announcement);

            await PressurePeopleForSomeTime(context, TimeSpan.FromSeconds(60));

            var (winners, max) = Booth.GetWinnersWithinAndScrap(options.Select(option => option.Id).ToList());
            var announcementModifier = new DiscordWebhookBuilder(announcement.WithContent(announcement.Content + "\n*This vote is closed.*"));
            announcementModifier.ClearComponents();
            await context.EditResponseAsync(announcementModifier);

            if (max == 0)
            {
                await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Nobody voted... :'("));
                return;
            }

            var formattedWinners = options.Where(option => winners.Contains(option.Id))
                .Select(option => option.Label)
                .JoinWith("* & *");
            await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("**The people has spoken** " +
                                                                                        $"and has elected *{formattedWinners}* " +
                                                                                        $"with {max} votes."));
        }
        catch (Exception e)
        {
            await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(e.Message));
        }
    }

    /// <summary>
    ///     Converts an index into a reaction emoji.
    /// </summary>
    /// <param name="index">Index of the reaction emoji.</param>
    /// <returns>Valid Discord emoji.</returns>
    private static string RegionalIndicatorFromIndex(int index)
    {
        return EmojiPrefix.Replace(EmojiPrefixPlaceholder, (char) (AAsciiIndex + index));
    }

    private static DiscordComponentEmoji DiscordComponentEmojiFromIndex(string name, BaseContext context)
    {
        return new DiscordComponentEmoji(DiscordEmoji.FromName(context.Client, name));
    }

    private static DiscordButtonComponent CreateDiscordButton(VoteOption option, DiscordComponentEmoji emoji)
    {
        return new DiscordButtonComponent(ButtonStyle.Secondary, option.Id, string.Empty, false, emoji);
    }

    private static async Task PressurePeopleForSomeTime(BaseContext context, TimeSpan time)
    {
        var remains = time;
        while (remains > TimeSpan.Zero)
        {
            var wait = remains.Minutes <= 1
                ? TimeSpan.FromSeconds(30)
                : TimeSpan.FromSeconds(60);
            var formattedRemaining = remains.Minutes <= 1
                ? $"{remains.TotalSeconds} seconds"
                : $"{remains.TotalMinutes} minutes";

            var message = await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(formattedRemaining + " left"));
            await Task.Delay(wait);
            await context.DeleteFollowupAsync(message.Id);

            remains = remains.Subtract(wait);
        }
    }

    private record VoteOption(string Label, string Id, string EmojiName);
}

internal record VoteAnswer(string OptionId, ulong UserId);

public class VoteAnswerEventHandler
{
    private readonly List<VoteAnswer> _answers = new();

    public void Register(string id)
    {
        //_answers.Add(new (id, ));
    }

    public (List<string>, int) GetWinnersWithinAndScrap(List<string> ids)
    {
        var ordered = _answers
            .GroupBy(tuple => tuple.OptionId)
            .Select(group => (group.Key, Total: group.Distinct().Count()))
            .OrderBy(tuple => -tuple.Total)
            .ToList();
        _answers.RemoveAll(answer => ids.Contains(answer.OptionId));

        return (ordered
                .Where(tuple => tuple.Total == ordered.First().Total)
                .Select(winner => winner.Key)
                .ToList(),
            ordered.First().Total);
    }

    public Task Handle(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        _answers.Add(new VoteAnswer(args.Id, args.User.Id));
        return Task.CompletedTask;
    }
}
