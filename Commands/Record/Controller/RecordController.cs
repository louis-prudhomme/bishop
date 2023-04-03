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

    [SlashCommand("randomin_user", "Returns someone's random record")]
    public async Task PickRandom(InteractionContext context,
        [OptionAttribute("user", "Which user ?")]
        DiscordUser user)
    {
        var picked = (await Manager.Find(user.Id)).Random();

        if (picked == null) await context.CreateResponseAsync("No history recorded.");
        else await context.CreateResponseAsync(Formatter.FormatRecord(picked));
    }

    [SlashCommand("randomin_category", "Returns a random record from the category")]
    public async Task PickRandom(InteractionContext context,
        [OptionAttribute("category", "Category to pull the records from")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Wind", (int) CounterCategory.Wind)]
        CounterCategory category)
    {
        var picked = (await Manager.Find(category)).Random();

        if (picked == null) await context.CreateResponseAsync("No history recorded.");
        else await context.CreateResponseAsync(Formatter.FormatRecord(picked));
    }

    [SlashCommand("consult", "To see the history of a @user")]
    public async Task Consult(InteractionContext context,
        [OptionAttribute("user", "@User to know the history of")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Wind", (int) CounterCategory.Wind)]
        CounterCategory category
    )
    {
        var records = await Manager.Find(user.Id, category);
        var score = await Manager.FindScore(user.Id, category);
        var rank = await Manager.FindRank(user.Id, category);

        if (records.IsEmpty())
        {
            await context.CreateResponseAsync($"No history recorded for user {user.Username} and category {category}");
        }
        else
        {
            var builder = new DiscordInteractionResponseBuilder
            {
                Content = Formatter.FormatLongRecord(user, category, rank, score, records.Take(GetLimit()))
            };

            await context.CreateResponseAsync(builder);
            using var figure = PlotManager.Cumulative(records).Image();
            builder.AddFile(figure.Stream());
            await context.EditResponseAsync(new DiscordWebhookBuilder(builder));
        }
    }

    [SlashCommand("since", "To see the history of a @user since a date")]
    public async Task Since(InteractionContext context,
        [OptionAttribute("user", "@User to know the progression of")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Wind", (int) CounterCategory.Wind)]
        CounterCategory counterCategory,
        [OptionAttribute("since", "Date from which compute progression")]
        TimeSpan? span
    )
    {
        if (span == null) return;
        var records = await Manager.Find(user.Id, counterCategory);
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
                Content = Formatter.FormatProgression(user, counterCategory, ratio, recordsSince, since)
            };

            await context.CreateResponseAsync(builder);
            using var figure = PlotManager.Cumulative(records).Image();
            builder.AddFile(figure.Stream());
            await context.EditResponseAsync(new DiscordWebhookBuilder(builder));
        }
    }

    [SlashCommand("blame", "To see the history of a @user")]
    public async Task Consult(InteractionContext context,
        [OptionAttribute("user", "@User to know the history of")]
        DiscordUser user
    )
    {
        var records = await Manager.Find(user.Id);

        if (records.Any())
            await context.CreateResponseAsync(records
                .Select(Formatter.FormatRecordWithCategory)
                .Take(GetLimit())
                .ToList());
        else await context.CreateResponseAsync($"No history recorded for user {user.Username}");
    }

    [SlashCommand("lastentries", "To see the history of a category")]
    public async Task Consult(InteractionContext context,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Wind", (int) CounterCategory.Wind)]
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