using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.History
{
    /// <summary>
    ///     The <c>History</c> class provides a set of commands to keep trace of user's deeds.
    /// </summary>
    class History : BaseCommandModule
    {
        [Command("history")]
        [Aliases("hy")]
        [Description("Returns the list of @someone’s history for a specific key (SAUCE/SEL/BDM)")]
        public async Task Prompt(CommandContext context,
            [Description("@User to know the history of")]
            DiscordMember member,
            [Description("Key to know the history of (SAUCE/SEL/BDM)")]
            MeterCategories meterCategory)
        {
            try
            {
                var history = Enumerat.FindAsync(member, meterCategory)
                    .Result.History
                    .Select(record => record.ToString())
                    .ToList();

                if (history.Count == 0)
                    await context.RespondAsync(
                        $"No history recorded for category user {member.Username} and {meterCategory}");
                else
                    await context.RespondAsync(history
                        .Aggregate((acc, h) => string.Join("\n", acc, h)));
            }
            catch (Exception e)
            {
                await context.RespondAsync(e.Message);
            }
        }

        [Command("history")]
        public async Task Add(CommandContext context,
            [Description("Operation to perform (Add)")]
            HistorySubcommandKey operation,
            [Description("@User to know the history of")]
            DiscordMember member,
            [Description("Key to know the history of (SAUCE/SEL/BDM)")]
            MeterCategories meterCategory,
            [Description("Record to add")] string history)
        {
            try
            {
                await context.RespondAsync(history);
                if (operation != HistorySubcommandKey.Add)
                    return;

                var record = Enumerat.FindAsync(member, meterCategory).Result;
                
                if (record.History == null)
                    record.History = new List<Record> {new(history)};
                else record.History.Add(new Record(history));

                await record.Commit();
                await context.RespondAsync($"Added «*{history}*» to {member.Username}’s {meterCategory.ToString().ToUpper()} history.");
            }
            catch (Exception e)
            {
                await context.RespondAsync(e.Message);
            }
        }
    }
}