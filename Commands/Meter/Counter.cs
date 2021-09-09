using System;
using System.Collections.Generic;
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
    internal partial class Counter : BaseCommandModule
    {
        [Command("score")]
        [Aliases("s")]
        [Description("Returns the list of @someone’s scores")]
        public async Task Score(CommandContext context,
            [Description("@User to know the scores of")]
            DiscordMember member)
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
        [Description("Returns the list of scores for a certain key")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
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
        [Description("Returns @someone’s score for a specific key")]
        public async Task Score(CommandContext context,
            [Description("@User to know the score of")]
            DiscordMember member,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
            MeterCategories meterCategory)
        {
            var score = Enumerat.FindAsync(member, meterCategory)
                .Result.ToString();
            await context.RespondAsync(score);
        }

        [Command("score")]
        [Description("Adds a provided value to @someone’s score")]
        public async Task Score(CommandContext context,
            [Description("User to add some score to")]
            DiscordMember member,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
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
        [Description("Adds a provided value to @someone’s score as well as a specific reason")]
        public async Task Score(CommandContext context,
            [Description("User to some score to")]
            DiscordMember member,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
            MeterCategories meterCategory,
            [Description("To increment by")] long nb,
            [RemainingText, Description("Context for the point(s) addition")] 
            string history)
        {
            var record = Enumerat.FindAsync(member, meterCategory).Result;

            var previous = record.Score;
            record.Score += nb;

            var pieces = history
                .Split(",")
                .Select(s => new Record(s))
                .ToList();
            if (record.History == null)
                record.History = pieces;
            else record.History.AddRange(pieces);

            await record.Commit();
            await context.RespondAsync($"{record} (from {previous}) (because of *« {history} »*)");
        }
    }
}