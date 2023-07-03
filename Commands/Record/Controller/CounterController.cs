using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Business;
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
    [SlashCommand("recap", "See every score of a user")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("user", "User to know the scores of")]
        DiscordUser user)
    {
        var records = await Manager.Find(user.Id);
        var scores = await Manager.FindScores(user.Id);

        if (!records.Any())
        {
            await context.CreateResponseAsync($"No scores for user {user.Username}");
        }
        else
        {
            var lines = await scores
                .Select(async pair => (Category: pair.Key, Score: pair.Value, Rank: await Manager.FindRank(user.Id, pair.Key)))
                .WhenAll(rankings => rankings.Select(ranking => Formatter.FormatSimpleRecordRanking(ranking.Rank, ranking.Category, ranking.Score)));

            var builder = new DiscordInteractionResponseBuilder
            {
                Content = Formatter.FormatRecap(user, lines)
            };
            await context.CreateResponseAsync(builder);

            using var figure = PlotManager.CumulativeBy(
                records,
                record => record.Category,
                record => record.DisplayName(),
                record => record.DisplayColor()).Image();
            builder.AddFile(figure.Stream());
            await context.EditResponseAsync(new DiscordWebhookBuilder(builder));
        }
    }

    [SlashCommand("podium", "See the podium in a specific category")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Rot", (int) CounterCategory.Rot)]
        [Choice("Pet", (int) CounterCategory.Pet)]
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

    [SlashCommand("score", "See someone's score in a certain category")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("user", "User to know the score of")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("Bdm", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Rot", (int) CounterCategory.Rot)]
        [Choice("Pet", (int) CounterCategory.Pet)]
        CounterCategory category)
    {
        var score = await Manager.Count(user.Id, category);

        await context.CreateResponseAsync(Formatter.FormatRecordRanking(user, category, score));
    }

    [SlashCommand("addmany", "Add many points to someone's history")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("user", "User to add the points to")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("Bdm", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Rot", (int) CounterCategory.Rot)]
        [Choice("Pet", (int) CounterCategory.Pet)]
        CounterCategory category,
        [OptionAttribute("points", "How many points ?")] [Maximum(10)] [Minimum(1)]
        long nb)
    {
        var records = Manager.CreateGhostRecords(user, category, nb);
        await RecordAndCreateResponseAsync(context, user, category, records);
    }

    [SlashCommand("add", "Add a record to someone's history")]
    public async Task Score(InteractionContext context,
        [OptionAttribute("user", "User to add the record to")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("Bdm", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        CounterCategory category,
        [OptionAttribute("reason", "Context for the point")]
        string motive)
    {
        if (int.TryParse(motive, out var result))
        {
            if (result is > 0 and < 11) await Score(context, user, category, result);
            return;
        }

        var record = new RecordEntity(user.Id, category, motive);
        await RecordAndCreateResponseAsync(context, user, category, new List<RecordEntity> {record});
    }

    private async Task RecordAndCreateResponseAsync(InteractionContext context, DiscordUser user, CounterCategory category, List<RecordEntity> records)
    {
        var (previous, current, nextMilestone) = await Manager.Save(user.Id, category, records);

        var reason = records.Count == 1 && records.First().Motive != null
            ? Formatter.FormatScoreUpdate(records.First().Motive!)
            : Formatter.FormatGhostScoreUpdate(records.Count);

        await context.CreateResponseAsync(Formatter.FormatRecordRankingUpdate(user, category, current, previous, reason));
        if (current >= nextMilestone) await context.CreateResponseAsync(Formatter.FormatBrokenMilestone(nextMilestone));
    }
}
