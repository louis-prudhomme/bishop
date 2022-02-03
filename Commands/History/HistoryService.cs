using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Meter;
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
public class HistoryService : BaseCommandModule
{
    public Random Random { private get; set; } = null!;
    public RecordRepository Repository { private get; set; } = null!;

    [GroupCommand]
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
        var originalUser = context.Guild.Members[picked.UserId];

        await context.RespondAsync($"• {picked.Motive} — {originalUser.Mention}");
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
        int? limit = -1
    )
    {
        var records = await Repository.FindByUserAndCategory(member.Id, counterCategory);
        var trueLimit = limit <= 0 ? records.Count : limit ?? records.Count;

        if (records.Any())
            await context.RespondAsync(records
                .Select(entity => entity.ToString())
                .Take(trueLimit)
                .Aggregate((acc, h) => string.Join("\n", acc, h)));
        else
            await context.RespondAsync(
                $"No history recorded for category user {member.Username} and {counterCategory}");
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordUser member,
        [Description("Number of records to pull")]
        int? limit = -1
    )
    {
        var records = await Repository.FindByUser(member.Id);
        var trueLimit = limit <= 0 ? records.Count : limit ?? records.Count;

        if (records.Any())
            await context.RespondAsync(records
                .Select(entity => entity.ToString())
                .Take(trueLimit)
                .Aggregate((acc, h) => string.Join("\n", acc, h)));
        else
            await context.RespondAsync(
                $"No history recorded for user {member.Username}");
    }
}