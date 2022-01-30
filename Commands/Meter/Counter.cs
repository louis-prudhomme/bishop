using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter
{
    /// <summary>
    ///     The <c>Counter</c> class provides a set of commands to keep trace of user's deeds.
    ///     This file contains all the general and generic commands ; alias for the commands are
    ///     provided in the separate file <c>CounterAliases.cs</c>.
    /// </summary>

    public class Counter : BaseCommandModule
    {
        [Command("score")]
        [Aliases("s")]
        [Description(
            "Allows interaction with @users’ scores. The scores can be seen by key or by @user, " +
            "and it is possible to add points to a player in a certain category. " +
            "It is also possible to add a reason for the point, which will then be in the @user’s history.")]
        public async Task Score(CommandContext context,
            [Description("Target @user")] DiscordMember member)
        {
            var scores = Enum.GetValues(typeof(MeterCategories))
                .OfType<MeterCategories>()
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
            MeterCategories meterCategory)
        {
            var scores = Enumerat.FindAllAsync(meterCategory).Result
                .Where(score => score != null)
                .Select(score => score.ToString())
                .ToList();

            if (!scores.Any())
                await context.RespondAsync($"No scores recorded for category {meterCategory}");
            else
                await context.RespondAsync(scores
                    .Aggregate((acc, score) => string.Join("\n", acc, score)));
        }

        [Command("score")]
        public async Task Score(CommandContext context,
            [Description("Target @user")] DiscordMember member,
            [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
            MeterCategories meterCategory)
        {
            var score = Enumerat.FindAsync(member, meterCategory)
                .Result.ToString();
            await context.RespondAsync(score);
        }

        [Command("score")]
        public async Task Score(CommandContext context,
            [Description("User to add some score to")]
            DiscordMember member,
            [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
            MeterCategories meterCategory,
            [Description("To increment by")] long nb)
        {
            var record = Enumerat.FindAsync(member, meterCategory).Result;

            var previous = record.Score;
            record.Score += nb;

            await record.Commit();
            await context.RespondAsync($"{record} (from {previous})");
        }

        [Command("score")]
        public async Task Score(CommandContext context,
            [Description("User to increment score of")]
            DiscordMember member,
            [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
            MeterCategories meterCategory,
            [RemainingText] [Description("Context for the point(s) addition")]
            string history)
        {
            await Task.WhenAll(Score(context, member, meterCategory, 1),
                new History.History().Add(context, member, meterCategory, history));
        }
    }
}