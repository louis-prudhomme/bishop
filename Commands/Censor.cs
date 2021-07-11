using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishop.Commands
{
    class Censor : BaseCommandModule
    {
        [Command("delete"), Aliases("d")]
        [Description("Deletes all the messages between the command and the one replied to.")]
        public async Task Delete(CommandContext context)
        {
            if (context.Message.MessageType == DSharpPlus.MessageType.Reply)
            {
                var limit = context.Message;
                var origin = limit.ReferencedMessage;

                var futures = context.Channel
                    .GetMessagesAfterAsync(origin.Id).Result
                    .TakeWhile(msg => msg.Timestamp > origin.Timestamp)
                    .Select(msg => msg.DeleteAsync())
                    .ToList();

                Task.WaitAll(futures.ToArray());
                await origin.DeleteAsync();
                await context.RespondAsync($"Removed {futures.Count + 1} 😉");
            }
            else await context.RespondAsync("You need to answer a message.");
        }
    }
}
