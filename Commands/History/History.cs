
using System;
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
    internal class History : BaseCommandModule
    {
        [Command("history")]
        [Aliases("hy")]
        [Description("Returns the list of @someone’s history for a specific key (SAUCE/SEL/BDM)")]
        public async Task Prompt(CommandContext context,
            [Description("@User to know the history of")]
            DiscordMember member, [Description("Key to know the history of (SAUCE/SEL/BDM")] MeterCategories meterCategory)
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
    }
}