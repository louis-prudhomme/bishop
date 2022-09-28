using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using Bishop.Commands.Meter.Aliases;
using Bishop.Config;
using Bishop.Helper;
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
    public ScoreFormatter ScoreFormatter { private get; set; } = new();
    public CounterRepository CounterRepository { private get; set; } = new();
    public RecordService HistoryService { private get; set; } = null!;

    [Command("score")]
    [Description(
        "Allows interaction with @users’ scores. The scores can be seen by key or by @user, " +
        "and it is possible to add points to a player in a certain category. " +
        "It is also possible to add a reason for the point, which will then be in the @user’s history.")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member)
    {
        var scores = await CounterRepository.FindByUser(member.Id);

        if (!scores.Any())
            await context.RespondAsync($"No scores for user {member.Username}");
        else
        {
            var entities = await Task.WhenAll(scores
                .Select((entity, i) => ScoreFormatter.Format(entity, i)));

            await context.RespondAsync(entities.JoinWithNewlines());
        }
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory counterCategory)
    {
        var scores = await CounterRepository.FindByCategory(counterCategory);

        if (!scores.Any())
        {
            await context.RespondAsync($"No scores for category {counterCategory}");
        }
        else
        {
            var entities = await Task.WhenAll(scores
                .Select((entity, i) => ScoreFormatter.Format(entity, i)));

            await context.RespondAsync(entities.JoinWithNewlines());
        }
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory counterCategory)
    {
        var score = await CounterRepository.FindOneByUserAndCategory(member.Id, counterCategory)
                    ?? new CounterEntity(member.Id, counterCategory);

        await context.RespondAsync(await ScoreFormatter.Format(member.Id, counterCategory, score));
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to add some score to")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory counterCategory,
        [Description("To increment by")] long nb)
    {
        var record = await CounterRepository
                         .FindOneByUserAndCategory(member.Id, counterCategory)
                     ?? new CounterEntity(member.Id, counterCategory);

        var previous = record.Score;
        record.Score += nb;

        await CounterRepository.SaveAsync(record);
        await HistoryService.AddGhostRecords(member, counterCategory, nb);

        var formatted = await ScoreFormatter.Format(record);
        await context.RespondAsync($"{formatted} (from {previous})");

        var milestone = GetNextMilestone(previous);
        if (record.Score >= milestone)
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
        await Task.WhenAll(
            Score(context, member, counterCategory, 1),
            HistoryService.Add(context, member, counterCategory, motive));
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