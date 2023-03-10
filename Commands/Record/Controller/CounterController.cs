using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Commands.Record.Controller.Aliases;
using Bishop.Helper.Extensions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordController</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
///     Classes specific to each category exist (ex: <see cref="SelCounterController" />).
/// </summary>
public partial class RecordController
{
    // TODO give rank of user for each metric
    [Command("score")]
    [Description(
        "Allows interaction with @users’ scores. The scores can be seen by key or by @user, " +
        "and it is possible to add points to a player in a certain category. " +
        "It is also possible to add a reason for the point, which will then be in the @user’s history.")]
    public async Task Score(CommandContext context, [Description("Target @user")] DiscordMember member)
    {
        var scores = await Manager.FindScores(member.Id);

        if (!scores.Any())
            await context.RespondAsync($"No scores for user {member.Username}");
        else
        {
            var lines = scores
                .Select(group => Formatter.FormatRecordRanking(member, group.Key, group.Value))
                .JoinWithNewlines();

            await context.RespondAsync(lines);
        }
    }

    [Command("score")]
    public async Task Score(CommandContext context, [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")] CounterCategory category)
    {
        var scores = await Manager.FindScores(category);

        if (!scores.Any())
        {
            await context.RespondAsync($"No scores for category {category}");
            return;
        }

        var scoresWithUserNames = await scores
            .Select(async tuple => (UserName: await Cache.GetValue(tuple.UserId) ?? "", tuple.Score))
            .WhenAll(list => list.ToList());
        var formattedRankings = Manager
            .RankScores(scoresWithUserNames)
            .Select(tuple => (UserName: tuple.Key, tuple.Score, tuple.Ranking))
            .Select(tuple => Formatter.FormatRecordRanking(tuple.UserName, category, tuple.Score, tuple.Ranking))
            .JoinWithNewlines();

        await context.RespondAsync(formattedRankings);
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory category)
    {
        var score = await Manager.Count(member.Id, category);

        await context.RespondAsync(Formatter.FormatRecordRanking(member, category, score));
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to add some score to")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory category,
        [Description("To increment by")] int nb)
    {
        if (nb <= 0)
        {
            await context.RespondAsync("Negative & null increments are not handled yet.");
            return;
        }

        var records = Manager.CreateGhostRecords(member, category, nb);
        await RecordAndRespondAsync(context, member, category, records);
    }

    [Command("score")]
    public async Task Score(CommandContext context,
        [Description("User to increment score of")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory category,
        [RemainingText] [Description("Context for the point(s) addition")]
        string motive)
    {
        var record = new RecordEntity(member.Id, category, motive);
        await RecordAndRespondAsync(context, member, category, new List<RecordEntity> {record});
        await context.RespondAsync(Formatter.FormatScoreUpdate(member, category, motive));
    }

    private async Task RecordAndRespondAsync(CommandContext context, DiscordMember member, CounterCategory category, List<RecordEntity> records)
    {
        var (previous, current, nextMilestone) = await Manager.Save(member.Id, category, records);

        await context.RespondAsync(Formatter.FormatRecordRankingUpdate(member, category, current, previous));
        if (current >= nextMilestone) await context.RespondAsync(Formatter.FormatBrokenMilestone(nextMilestone));
    }
}