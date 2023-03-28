using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Controller.Aliases;
using Bishop.Commands.Record.Domain;
using Bishop.Helper.Extensions;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordController</c> class provides a set of commands to keep trace of user's
///     deeds.
///     This file contains all the general and generic commands.
///     Classes specific to each category exist (ex: <see cref="SelCounterController" />).
/// </summary>
public partial class RecordController
{
    // TODO give rank of user for each metric
    [SlashCommand("score_user", "placeholder")]
    public async Task Score(InteractionContext context, [OptionAttribute("member", "Target @user")] DiscordUser member)
    {
        var scores = await Manager.FindScores(member.Id);

        if (!scores.Any())
        {
            await context.CreateResponseAsync($"No scores for user {member.Username}");
        }
        else
        {
            var lines = scores
                .Select(group => Formatter.FormatRecordRanking(member, group.Key, group.Value))
                .JoinWith(RecordFormatter.TabulatedNewline);

            await context.CreateResponseAsync(lines);
        }
    }

    [SlashCommand("score_category", "placeholder")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("category", "Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory category)
    {
        var scores = await Manager.FindScores(category);

        if (!scores.Any())
        {
            await context.CreateResponseAsync($"No scores for category {category}");
        }
        else
        {
            var scoresWithUserNames = await scores
                .Select(async tuple => (UserName: await Cache.GetValue(tuple.UserId) ?? "", tuple.Score))
                .WhenAll(list => list.ToList());
            var formattedRankings = Manager
                .RankScores(scoresWithUserNames)
                .Select(tuple => (UserName: tuple.Key, tuple.Score, tuple.Ranking))
                .Select(tuple => Formatter.FormatRecordRanking(tuple.UserName, category, tuple.Score, tuple.Ranking))
                .JoinWithNewlines();

            await context.CreateResponseAsync(formattedRankings);
        }
    }

    [SlashCommand("score", "placeholder")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("member", "Target @user")]
        DiscordUser member,
        [OptionAttribute("category", "Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory category)
    {
        var score = await Manager.Count(member.Id, category);

        await context.CreateResponseAsync(Formatter.FormatRecordRanking(member, category, score));
    }

    [SlashCommand("addmany", "placeholder")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("member", "User to add some score to")]
        DiscordUser member,
        [OptionAttribute("category", "Target key (must be BDM/Beauf/Sauce/Sel/Rass...)")]
        CounterCategory category,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        switch (nb)
        {
            case <= 0:
                await context.CreateResponseAsync("Negative & null increments are not handled yet.");
                break;
            case > 10:
                await context.CreateResponseAsync("This is probably an error, fix your shit.");
                break;
            default:
            {
                var records = Manager.CreateGhostRecords(member, category, nb);
                await RecordAndCreateResponseAsync(context, member, category, records);
                break;
            }
        }
    }

    [SlashCommand("add", "placeholder")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("member", "User to increment score of")]
        DiscordUser member,
        [OptionAttribute("category", "Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory category,
        [OptionAttribute("motive", "Context for the point(s) addition")]
        string motive)
    {
        var record = new RecordEntity(member.Id, category, motive);
        await RecordAndCreateResponseAsync(context, member, category, new List<RecordEntity> {record});
    }

    private async Task RecordAndCreateResponseAsync(InteractionContext context, DiscordUser member, CounterCategory category, List<RecordEntity> records)
    {
        var (previous, current, nextMilestone) = await Manager.Save(member.Id, category, records);

        var reason = records.Count == 1 && records.First().Motive != null
            ? Formatter.FormatScoreUpdate(records.First().Motive!)
            : Formatter.FormatGhostScoreUpdate(records.Count);

        await context.CreateResponseAsync(Formatter.FormatRecordRankingUpdate(member, category, current, previous, reason));
        if (current >= nextMilestone) await context.CreateResponseAsync(Formatter.FormatBrokenMilestone(nextMilestone));
    }
}