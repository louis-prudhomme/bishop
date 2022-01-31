using System;
using System.Collections.Generic;
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
internal class History : BaseCommandModule
{
    public Random Random { private get; set; }
    public RecordRepository RecordRepository { private get; set; } = null!;

    [GroupCommand]
    [Description("Picks a random record to expose")]
    public async Task PickRandom(CommandContext context)
    {
        var records = (await RecordRepository.FindAllAsync()).ToList();
        
        if (!records.Any()) 
        {
            await context.RespondAsync("No history recorded.");
            return;
        }

        var picked = records.ElementAt(Random.Next(0, records.Count));
        // TODO default value as l'étranger
        var originalUser = context.Guild.Members[picked.UserId];
        
        await context.RespondAsync($"{picked.Motive} — {originalUser.Mention}");
    }

    [Command("add")]
    [Aliases("a")]
    [Description("To add a new record to a @member’s history")]
    public async Task Add(CommandContext context,
        [Description("@User to add the record to")]
        DiscordMember member,
        [Description("Key to add the record to")]
        CountCategory countCategory,
        [Description("Record to add")] [RemainingText]
        string motive)
    {
        var record = new RecordEntity(member.Id, countCategory, motive);

        await RecordRepository.SaveAsync(record);
        await context.RespondAsync(
            $"Added «*{record.Motive}*» to {member.Mention}’s {countCategory} history.");
    }

    [Command("consult")]
    [Aliases("c")]
    [Description("To see the history of a @member")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member,
        [Description("Category to know the history of")]
        CountCategory countCategory,
        [Description("Number of records to pull")] int limit
    )
    {
        var records = await RecordRepository.FindByUserAndCategory(member.Id, countCategory);
        var trueLimit = limit > -1 ? records.Count : limit;
        
        if (records.Any())
            await context.RespondAsync(records
                .Select(entity => entity.ToString())
                .Take(trueLimit)
                .Aggregate((acc, h) => string.Join("\n", acc, h)));
        else
            await context.RespondAsync(
                $"No history recorded for category user {member.Username} and {countCategory}");
    }
    
    [Command("consult")]
    [Description("To see the history of a @member")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member,
        [Description("Category to know the history of")]
        CountCategory countCategory
    )
    {
        await Consult(context, member, countCategory, -1);
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member,
        [Description("Number of records to pull")] int limit
    )
    {
        var records = await RecordRepository.FindByUser(member.Id);
        var trueLimit = limit > -1 ? records.Count : limit;

        if (records.Any())
            await context.RespondAsync(records
                .Select(entity => entity.ToString())
                .Take(trueLimit)
                .Aggregate((acc, h) => string.Join("\n", acc, h)));
        else
            await context.RespondAsync(
                $"No history recorded for user {member.Username}");
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member
    )
    {
        await Consult(context, member, -1);
    }
}