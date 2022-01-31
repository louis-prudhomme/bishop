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

    [GroupCommand]
    [Description("Picks a random record to expose")]
    public async Task PickRandom(CommandContext context)
    {
        var enumerats = Enumerat.FindAllWithHistoryAsync().Result
            .Where(enumerat => enumerat.History != null)
            .SelectMany(enumerat => enumerat.History.Select(record => new {enumerat.User, record}))
            .ToList();
        if (enumerats.Count == 0)
        {
            await context.RespondAsync("No history recorded.");
            return;
        }

        var picked = enumerats.ElementAt(Random.Next(0, enumerats.Count));

        await context.RespondAsync($"{picked.record} — {picked.User}");
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
        string history)
    {
        var record = Enumerat.FindAsync(member, countCategory).Result;

        if (record.History == null)
            record.History = new List<Record> {new(history)};
        else record.History.Add(new Record(history));

        await record.Commit();
        await context.RespondAsync(
            $"Added «*{history}*» to {member.Username}’s {countCategory} history.");
    }

    [Command("consult")]
    [Aliases("c")]
    [Description("To see the history of a @member")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member,
        [Description("Key to know the history of")]
        CountCategory countCategory
    )
    {
        var history = Enumerat.FindAsync(member, countCategory)
            .Result.History
            .Select(record => record.ToString())
            .ToList();

        if (history.Count == 0)
            await context.RespondAsync(
                $"No history recorded for category user {member.Username} and {countCategory}");
        else
            await context.RespondAsync(history
                .Aggregate((acc, h) => string.Join("\n", acc, h)));
    }

    [Command("consult")]
    private async Task Consult(CommandContext context,
        [Description("@User to know the history of")]
        DiscordMember member
    )
    {
        var history = Enumerat.FindAllWithHistoryAsync(member)
            .Result.SelectMany(enumerat => enumerat.History)
            .Select(record => record.ToString())
            .ToList();

        if (history.Count == 0)
            await context.RespondAsync(
                $"No history recorded for user {member.Username}");
        else
            await context.RespondAsync(history.ElementAt(Random.Next(0, history.Count)));
    }
}