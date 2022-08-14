using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Meter;
using Bishop.Config;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.History;

/// <summary>
///     The <c>History</c> class provides a set of commands to keep trace of user's deeds.
/// </summary>
[Group("history")]
[Aliases("hy")]
[Description("History-related commands")]
public class RecordService : BaseCommandModule
{
    private const int DefaultLimit = 10;
    public Random Random { private get; set; } = null!;
    public UserNameCache Cache { private get; set; } = null!;
    public RecordRepository Repository { private get; set; } = null!;

    [Command("rand")]
    [Description("Picks a random record to expose")]
    public async Task PickRandom(CommandContext context)
    {
        var records = (await Repository.FindAllAsync()).ToList();

        if (!records.Any())
        {
            await context.RespondAsync("No history recorded.");
            return;
        }

        var picked = records.ElementAt(Random.Next(0, records.Count));
        // TODO default value as l'étranger
        var originalUser = await Cache.GetAsync(picked.UserId);

        await context.RespondAsync($"«*{picked.Motive}*» — {originalUser}");
    }

    [Command("rand")]
    [Description("Returns a @member's random record")]
    public async Task ConsultShort(CommandContext context, DiscordMember member)
    {
        var records = await Repository.FindByUser(member.Id);
        if (!records.Any())
        {
            await context.RespondAsync("No history recorded.");
            return;
        }

        var picked = records.ElementAt(Random.Next(0, records.Count));
        // TODO default value as l'étranger
        var originalUser = await Cache.GetAsync(picked.UserId);

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
        var trueLimit = limit <= 0 ? records.Count : limit ?? records.Count;

        if (records.Any())
            await FormatRecordList(context, records, trueLimit);
        else
            await context.RespondAsync(
                $"No history recorded for category user {member.Username} and {counterCategory}");
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordUser member,
        [Description("Number of records to pull")]
        int? limit = DefaultLimit
    )
    {
        var records = await Repository.FindByUser(member.Id);

        if (records.Any())
            await FormatRecordList(context, records, limit ?? DefaultLimit);
        else
            await context.RespondAsync(
                $"No history recorded for user {member.Username}");
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("Category to pull records of")]
        CounterCategory category,
        [Description("Number of records to pull")]
        int? limit = DefaultLimit
    )
    {
        var records = await Repository.FindByCategory(category);

        if (records.Any())
            await FormatRecordList(context, records, limit ?? DefaultLimit);
        else
            await context.RespondAsync(
                $"No history recorded for category {category}");
    }

    private static async Task FormatRecordList(CommandContext context, IReadOnlyCollection<RecordEntity> records, int limit)
    {
        var trueLimit = limit <= 0 ? records.Count : limit;

        var toSend = records
            .Select(record => record.ToString())
            .Take(trueLimit)
            .ToList();

        await DiscordMessageCutter.PaginateAnswer(toSend, context.RespondAsync);
    }
}