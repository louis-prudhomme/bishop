using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        var originalUser = await Cache.GetAsync(picked.UserId);

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
            await FormatRecordList(context, records, trueLimit, false);
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
                await context.RespondAsync($"There's no progression for you in {counterCategory.ToString().ToLower()} since {DateHelper.FromDateTimeToStringDate(since)}. How sad...");
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
        int? limit = DefaultLimit
    )
    {
        var records = await Repository.FindByUser(member.Id);

        if (records.Any())
            await FormatRecordList(context, records, limit ?? DefaultLimit, true);
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
            await FormatRecordList(context, records, limit ?? DefaultLimit, false);
        else
            await context.RespondAsync(
                $"No history recorded for category {category}");
    }

    public async Task AddGhostRecords(SnowflakeObject member, CounterCategory category, long nb)
    {
        var recordsToInsert = new List<RecordEntity>();
        for (var i = 0; i < nb; i++)
        {
            recordsToInsert.Add(new RecordEntity(member.Id, category, null));
        }

        await Repository.InsertManyAsync(recordsToInsert);
    }

    private static async Task FormatRecordList(CommandContext context,
        IReadOnlyCollection<RecordEntity> records,
        int limit,
        bool shouldIncludeCategory)
    {
        var trueLimit = limit <= 0 ? records.Count : limit;

        var toSend = records
            .Select(record => record.ToString(shouldIncludeCategory))
            .Take(trueLimit)
            .ToList();

        await DiscordMessageCutter.PaginateAnswer(toSend, context.RespondAsync);
    }
}