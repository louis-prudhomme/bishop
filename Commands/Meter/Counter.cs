using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using Bishop.Commands.Meter;

namespace Bishop.Commands.Meter
{
    partial class Counter : BaseCommandModule
    {
        [Command("score"), Aliases("s")]
        [Description("Returns the list of @someone’s scores")]
        public async Task Score(CommandContext context,
            [Description("@User to know the scores of")] DiscordMember member)
        {
            var scores = Enum.GetValues(typeof(Keys))
            .OfType<Keys>()
            .Select(key => Enumerat.FindAsync(member.Username, key)
                .Result)
            .Where(key => key != null)
            .Select(key => key.ToString());

            if (!scores.Any())
                await context.RespondAsync($"No scores for user {member.Username}");
            else
                await context.RespondAsync(scores
                    .Aggregate((key1, key2) => string.Join("\n", key1, key2)));
        }

        [Command("score")]
        [Description("Returns the list of scores for a certain key")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")] Keys key)
        {
            var scores = Enumerat.FindAllAsync(key).Result
                .Where(score => score != null)
                .Select(score => score.ToString());

            if (!scores.Any())
                await context.RespondAsync($"No scores recorded for category {key}");
            else await context.RespondAsync(scores
                .Aggregate((acc, score) => string.Join("\n", acc, score)));
        }

        [Command("score")]
        [Description("Returns @someone’s score for a specific key")]
        public async Task Score(CommandContext context,
            [Description("@User to know the score of")] DiscordMember member,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")] Keys key)
        {
            var score = Enumerat.FindAsync(member.Username, key)
               .Result.ToString();
            await context.RespondAsync(score);
        }

        [Command("score")]
        [Description("Adds a provided value to @someone’s score")]
        public async Task Score(CommandContext context,
            [Description("User to know the score of")] DiscordMember member,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")] Keys key,
            [Description("To increment by")] int nb)
        {
            var record = Enumerat.FindAsync(member.Username, key).Result;
            var previous = record.Score;
            record.Score += nb;

            await record.Commit();
            await context.RespondAsync($"{record} (from {previous})");
        }
    }
}
