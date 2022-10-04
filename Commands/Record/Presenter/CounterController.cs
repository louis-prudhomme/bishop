﻿using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Commands.Record.Model;
using Bishop.Commands.Record.Presenter.Aliases;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Presenter;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordService</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
///     Classes specific to each category exist (ex: <see cref="SelCounterController" />).
/// </summary>
public partial class RecordController : BaseCommandModule
{
    public ScoreFormatter ScoreFormatter { private get; set; } = new();
    public RecordRepository RecordRepository { private get; set; } = new();

    // TODO give rank of user for each metric
    [Command("score")]
    [Description(
        "Allows interaction with @users’ scores. The scores can be seen by key or by @user, " +
        "and it is possible to add points to a player in a certain category. " +
        "It is also possible to add a reason for the point, which will then be in the @user’s history.")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member)
    {
        var scores = await RecordRepository.CountByUserGroupByCategory(member.Id);

        if (!scores.Any())
            await context.RespondAsync($"No scores for user {member.Username}");
        else
        {
            var lines = scores
                .Select(group => ScoreFormatter.Format(member, group.Key, group.Value))
                .JoinWithNewlines();

            await context.RespondAsync(lines);
        }
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory counterCategory)
    {
        var scores = await RecordRepository
            .CountByCategoryGroupByUser(counterCategory);

        if (!scores.Any())
            await context.RespondAsync($"No scores for category {counterCategory}");
        else
        {
            var entities = await Task.WhenAll(scores
                .Select(pair => pair)
                .OrderByDescending(pair => pair.Value)
                .Select((pair, i) => ScoreFormatter.Format(pair.Key, counterCategory, pair.Value, i)));

            await context.RespondAsync(entities.JoinWithNewlines());
        }
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory counterCategory)
    {
        var score = await RecordRepository.CountByUserAndCategory(member.Id, counterCategory);

        await context.RespondAsync(ScoreFormatter.Format(member, counterCategory, score));
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to add some score to")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory counterCategory,
        [Description("To increment by")] long nb)
    {
        if (nb <= 0)
        { 
            await context.RespondAsync("Negative & null increments are not handled yet.");
            return;
        }
        var previous = await RecordRepository
            .CountByUserAndCategory(member.Id, counterCategory);

        await AddGhostRecords(member, counterCategory, nb);

        var formatted = ScoreFormatter.Format(member, counterCategory, previous + nb);
        await context.RespondAsync($"{formatted} (from {previous})");

        var milestone = GetNextMilestone(previous);
        if (previous + nb >= milestone)
            await context.RespondAsync($"A new milestone has been broken through: {milestone}! 🎉");
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to increment score of")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory counterCategory,
        [RemainingText] [Description("Context for the point(s) addition")]
        string motive)
    {
        await Add(context, member, counterCategory, motive);
    }

    private static long GetNextMilestone(long current)
    {
        return current switch
        {
            < 10 => 10,
            < 50 => 50,
            < 100 => 100,
            _ => (current / 100 + 1) * 100
        };
    }
}