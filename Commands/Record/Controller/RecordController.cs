using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>History</c> class provides a set of commands to keep trace of user's deeds.
/// </summary>
[Group("history")]
[Aliases("hy")]
[Description("History-related commands")]
public partial class RecordController : BaseCommandModule
{
    private const int DefaultLimit = 10;

    [Command("random")]
    [Aliases("r")]
    [Description("Picks a random record to expose")]
    public async Task PickRandom(CommandContext context)
    {
        var picked = (await Manager.GetAllNonNulls()).Random();

        if (picked == null) await context.RespondAsync("No history recorded.");
        else await context.RespondAsync(Formatter.FormatRecord(picked));
    }

    [Command("random")]
    [Description("Returns a @member's random record")]
    public async Task PickRandom(CommandContext context, DiscordMember member)
    {
        var picked = (await Manager.Find(member.Id)).Random();

        if (picked == null) await context.RespondAsync("No history recorded.");
        else await context.RespondAsync(Formatter.FormatRecord(picked));
    }

    [Command("random")]
    [Description("Returns a random record from the category")]
    public async Task PickRandom(CommandContext context, CounterCategory category)
    {
        var picked = (await Manager.Find(category)).Random();

        if (picked == null) await context.RespondAsync("No history recorded.");
        else await context.RespondAsync(Formatter.FormatRecord(picked));
    }

    [Command("consult")]
    [Aliases("c")]
    [Description("To see the history of a @member")]
    public async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member,
        [Description("Category to know the history of")]
        CounterCategory category,
        [Description("Number of records to pull")]
        int? limit
    )
    {
        var records = await Manager.Find(member.Id, category);
        var score = await Manager.FindScore(member.Id, category);
        var rank = await Manager.FindRank(member.Id, category);

        if (records.Any()) await context.RespondAsync(Formatter.FormatLongRecord(member, category, rank + 1, score, records.Take(GetLimit(limit))));
        else await context.RespondAsync($"No history recorded for user {member.Username} and category {category}");
    }

    [Command("since")]
    [Aliases("s")]
    [Description("To see the history of a @member since a date")]
    public async Task Since(CommandContext context,
        [Description("@User to know the progression of")]
        DiscordMember member,
        [Description("Category to know the history of")]
        CounterCategory counterCategory,
        [Description("Date from which compute progression")]
        DateTime since
    )
    {
        var records = await Manager.Find(member.Id, counterCategory);
        var recordsSince = records.Select(record => record.RecordedAt >= since).Count();

        if (records.IsEmpty())
        {
            await context.RespondAsync("No progression to measure on nothing, cunt.");
            return;
        }

        var ratio = recordsSince / records.Count;
        await context.RespondAsync(Formatter.FormatProgression(member, counterCategory, ratio, recordsSince, since));
    }

    [Command("consult")]
    public async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordUser member,
        [Description("Number of records to pull")]
        int? limit
    )
    {
        var records = await Manager.Find(member.Id);

        if (records.Any())
            await context.RespondAsync(records
                .Select(Formatter.FormatRecordWithCategory)
                .Take(GetLimit(limit))
                .ToList());
        else await context.RespondAsync($"No history recorded for user {member.Username}");
    }

    [Command("consult")]
    public async Task Consult(CommandContext context,
        [Description("Category to pull records of")]
        CounterCategory category,
        [Description("Number of records to pull")]
        int? limit
    )
    {
        var records = await Manager.Find(category);

        if (records.Any())
            await context.RespondAsync(records
                .Select(Formatter.FormatRecord)
                .Take(GetLimit(limit))
                .ToList());
        else await context.RespondAsync($"No history recorded for category {category}");
    }

    private static int GetLimit(int? preference) => preference ?? DefaultLimit;
}