using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using Bishop.Commands.Meter.Aliases;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter;

/// <summary>
///     The <c>Counter</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
///     Classes specific to each category exist (ex: <see cref="SelCounter" />).
/// </summary>
public class CounterService : BaseCommandModule
{
    public CounterRepository Repository { private get; set; } = null!;
    public HistoryService Service { private get; set; } = null!;

    [Command("score")]
    [Aliases("s")]
    [Description(
        "Allows interaction with @users’ scores. The scores can be seen by key or by @user, " +
        "and it is possible to add points to a player in a certain category. " +
        "It is also possible to add a reason for the point, which will then be in the @user’s history.")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member)
    {
        var scores = Enum.GetValues(typeof(CountCategory))
            .OfType<CountCategory>()
            .Select(key => Enumerat.FindAsync(member, key)
                .Result)
            .Where(key => key != null)
            .Select(key => key.ToString())
            .ToList();

        if (!scores.Any())
            await context.RespondAsync($"No scores for user {member.Username}");
        else
            await context.RespondAsync(scores
                .Aggregate((key1, key2) => string.Join("\n", key1, key2)));
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CountCategory countCategory)
    {
        var scores = Enumerat.FindAllAsync(countCategory).Result
            .Where(score => score != null)
            .Select(score => score.ToString())
            .ToList();

        if (!scores.Any())
            await context.RespondAsync($"No scores recorded for category {countCategory}");
        else
            await context.RespondAsync(scores
                .Aggregate((acc, score) => string.Join("\n", acc, score)));
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CountCategory countCategory)
    {
        var score = await Repository.FindByUserAndCategory(member.Id, countCategory)
                    ?? new CounterEntity(member.Id, countCategory);
        await context.RespondAsync(score.ToString());
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to add some score to")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CountCategory countCategory,
        [Description("To increment by")] long nb)
    {
        var record = await Repository.FindByUserAndCategory(member.Id, countCategory)
                     ?? new CounterEntity(member.Id, countCategory);

        var previous = record.Score;
        record.Score += nb;

        await Repository.SaveAsync(record);
        await context.RespondAsync($"{record} (from {previous})");
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to increment score of")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CountCategory countCategory,
        [RemainingText] [Description("Context for the point(s) addition")]
        string motive)
    {
        await Task.WhenAll(
            Score(context, member, countCategory, 1),
            Service.Add(context, member, countCategory, motive));
    }
}