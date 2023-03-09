﻿using System;
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
    public RecordRepository Repository { private get; set; } = null!;

    [Command("rand")]
    [Aliases("r")]
    [Description("Picks a random record to expose")]
    public async Task PickRandom(CommandContext context)
    {
        var records = (await Repository.FindAllAsync())
            .Where(record => record.Motive != null)
            .ToList();

        if (!records.Any())
        {
            await context.RespondAsync("No history recorded.");
            return;
        }

        var picked = records.ElementAt(Random.Next(0, records.Count));
        // TODO default value as l'étranger
        var originalUser = await Cache.Get(picked.UserId);

        await context.RespondAsync($"«*{picked.Motive}*» — {originalUser}");
    }

    [Command("rand")]
    [Description("Returns a @member's random record")]
    public async Task ConsultShort(CommandContext context, DiscordMember member)
    {
        var records = (await Repository.FindByUser(member.Id))
            .Where(record => record.Motive != null)
            .ToList();

        if (!records.Any())
        {
            await context.RespondAsync("No history recorded.");
            return;
        }

        var picked = records.ElementAt(Random.Next(0, records.Count));
        // TODO default value as l'étranger
        var originalUser = await Cache.Get(picked.UserId);

        await context.RespondAsync($"«*{picked.Motive}*» — {originalUser}");
    }

    [Command("add")]
    [Aliases("a")]
    [Description("To add a new record to a @member’s history")]
    public async Task Add(CommandContext context,
        [Description("@User to add the record to")]
        DiscordMember member,
        [Description("Category to add the record to")]
        CounterCategory counterCategory,
        [Description("Record to add")] [RemainingText]
        string motive)
    {
        var record = new RecordEntity(member.Id, counterCategory, motive);

        await Repository.SaveAsync(record);
        await context.RespondAsync(
            $"Added «*{record.Motive}*» to {member.Mention}’s {counterCategory} history.");
    }

    [Command("consult")]
    [Aliases("c")]
    [Description("To see the history of a @member")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordUser member,
        [Description("Category to know the history of")]
        CounterCategory counterCategory,
        [Description("Number of records to pull")]
        int? limit = DefaultLimit
    )
    {
        var records = await Repository.FindByUserAndCategory(member.Id, counterCategory);
        var trueLimit = GetLimit(limit, records.Count);

        if (records.Any())
            await context.RespondAsync(records
                .Select(Formatter.FormatRecord)
                .Take(trueLimit)
                .ToList());
        else
            await context.RespondAsync(
                $"No history recorded for category user {member.Username} and {counterCategory}");
    }

    [Command("consult")]
    [Aliases("s")]
    [Description("To see the history of a @member")]
    private async Task Since(CommandContext context,
        [Description("@User to know the progression of")]
        DiscordUser member,
        [Description("Category to know the history of")]
        CounterCategory counterCategory,
        [Description("Date from which compute progression")]
        DateTime since
    )
    {
        var records = await Repository.FindByUserAndCategory(member.Id, counterCategory);
        var total = Convert.ToDouble(records.Count);
        var recordsSince = Convert.ToDouble(records.Select(record => record.RecordedAt >= since).Count());

        if (total == 0)
        {
            await context.RespondAsync("No progression to measure on nothing, cunt.");
            return;
        }

        var ratio = recordsSince / total;
        switch (ratio)
        {
            case 0:
                await context.RespondAsync(
                    $"There's no progression for you in {counterCategory.ToString().ToLower()} since {DateHelper.FromDateTimeToStringDate(since)}. How sad...");
                return;
            case > 0.1:
                await context.RespondAsync(
                    $"You're on fire, {member.Mention}'s ! {recordsSince} points in {counterCategory.ToString().ToLower()} since {DateHelper.FromDateTimeToStringDate(since)}");
                return;
            default:
                await context.RespondAsync(
                    $"{member.Mention} gained {recordsSince} in {counterCategory.ToString().ToLower()} points since {DateHelper.FromDateTimeToStringDate(since)}");
                return;
        }
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordUser member,
        [Description("Number of records to pull")]
        int? limit
    )
    {
        var records = await Repository.FindByUser(member.Id);
        var trueLimit = GetLimit(limit, records.Count);

        if (records.Any())
            await context.RespondAsync(records
                .Select(Formatter.FormatRecordWithCategory)
                .Take(trueLimit)
                .ToList());
        else
            await context.RespondAsync(
                $"No history recorded for user {member.Username}");
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("Category to pull records of")]
        CounterCategory category,
        [Description("Number of records to pull")]
        int? limit
    )
    {
        var records = await Repository.FindByCategory(category);
        var trueLimit = GetLimit(limit, records.Count);

        if (records.Any())
            await context.RespondAsync(records
                .Select(Formatter.FormatRecord)
                .Take(trueLimit)
                .ToList());
        else
            await context.RespondAsync(
                $"No history recorded for category {category}");
    }
    
    private static int GetLimit(int? preference, int recordsCount) => preference <= 0 ? recordsCount : preference ?? DefaultLimit;
}