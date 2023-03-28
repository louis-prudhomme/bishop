using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper.Extensions;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>History</c> class provides a set of commands to keep trace of user's deeds.
/// </summary>
[SlashCommandGroup("history", "History-related commands")]
public partial class RecordController : ApplicationCommandModule
{
    private const int DefaultLimit = 10;

    [SlashCommand("random", "Picks a random record to expose")]
    public async Task PickRandom(InteractionContext context)
    {
        var picked = (await Manager.GetAllNonNulls()).Random();

        if (picked == null) await context.CreateResponseAsync("No history recorded.");
        else await context.CreateResponseAsync(Formatter.FormatRecord(picked));
    }

    [SlashCommand("random_user", "Returns a @member's random record")]
    public async Task PickRandom(InteractionContext context,
        [OptionAttribute("user", "placeholder")]
        DiscordUser member)
    {
        var picked = (await Manager.Find(member.Id)).Random();

        if (picked == null) await context.CreateResponseAsync("No history recorded.");
        else await context.CreateResponseAsync(Formatter.FormatRecord(picked));
    }

    [SlashCommand("random_category", "Returns a random record from the category")]
    public async Task PickRandom(InteractionContext context,
        [OptionAttribute("category", "placeholder")]
        CounterCategory category)
    {
        var picked = (await Manager.Find(category)).Random();

        if (picked == null) await context.CreateResponseAsync("No history recorded.");
        else await context.CreateResponseAsync(Formatter.FormatRecord(picked));
    }

    [SlashCommand("consult", "To see the history of a @member")]
    public async Task Consult(InteractionContext context,
        [OptionAttribute("member", "@User to know the history of")]
        DiscordUser member,
        [OptionAttribute("category", "Category to know the history of")]
        CounterCategory category
    )
    {
        var records = await Manager.Find(member.Id, category);
        var score = await Manager.FindScore(member.Id, category);
        var rank = await Manager.FindRank(member.Id, category);

        if (records.IsEmpty())
        {
            await context.CreateResponseAsync($"No history recorded for user {member.Username} and category {category}");
        }
        else
        {
            var builder = new DiscordInteractionResponseBuilder
            {
                Content = Formatter.FormatLongRecord(member, category, rank, score, records.Take(GetLimit()))
            };

            await context.CreateResponseAsync(builder);
            using var figure = PlotManager.Cumulative(records).Image();
            builder.AddFile(figure.Stream());
            await context.EditResponseAsync(new DiscordWebhookBuilder(builder));
        }
    }

    [SlashCommand("since", "To see the history of a @member since a date")]
    public async Task Since(InteractionContext context,
        [OptionAttribute("member", "@User to know the progression of")]
        DiscordUser member,
        [OptionAttribute("category", "Category to know the history of")]
        CounterCategory counterCategory,
        [OptionAttribute("since", "Date from which compute progression")]
        TimeSpan? span
    )
    {
        if (span == null) return;
        var records = await Manager.Find(member.Id, counterCategory);
        var since = DateTime.Now.Subtract(span.Value);
        var recordsSince = records.Select(record => record.RecordedAt >= since).Count();

        if (records.IsEmpty())
        {
            await context.CreateResponseAsync("No progression to measure on nothing, cunt.");
        }
        else
        {
            var ratio = recordsSince / records.Count;
            var builder = new DiscordInteractionResponseBuilder
            {
                Content = Formatter.FormatProgression(member, counterCategory, ratio, recordsSince, since)
            };

            await context.CreateResponseAsync(builder);
            using var figure = PlotManager.Cumulative(records).Image();
            builder.AddFile(figure.Stream());
            await context.EditResponseAsync(new DiscordWebhookBuilder(builder));
        }
    }

    [SlashCommand("for_user", "To see the history of a @member")]
    public async Task Consult(InteractionContext context,
        [OptionAttribute("user", "@User to know the history of")]
        DiscordUser member
    )
    {
        var records = await Manager.Find(member.Id);

        if (records.Any())
            await context.CreateResponseAsync(records
                .Select(Formatter.FormatRecordWithCategory)
                .Take(GetLimit())
                .ToList());
        else await context.CreateResponseAsync($"No history recorded for user {member.Username}");
    }

    [SlashCommand("for_category", "To see the history of a category")]
    public async Task Consult(InteractionContext context,
        [OptionAttribute("category", "Category to pull records of")]
        CounterCategory category
    )
    {
        var records = await Manager.Find(category);

        if (records.Any())
            await context.CreateResponseAsync(records
                .Select(Formatter.FormatRecord)
                .Take(GetLimit())
                .ToList());
        else await context.CreateResponseAsync($"No history recorded for category {category}");
    }

    private static int GetLimit()
    {
        return DefaultLimit; //FIXME : i do not work as of right now
    }
}