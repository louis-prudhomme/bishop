using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands
{
    /// <summary>
    ///     This class provides a set of commands to delete messages.
    /// </summary>
    internal class Deleter : BaseCommandModule
    {
        [Command("delete")]
        [Aliases("d")]
        [Description("Deletes all the messages between the command and the one replied to.")]
        public async Task Delete(CommandContext context, [RemainingText] string silentFlag)
        {
            if (context.Message.MessageType == MessageType.Reply)
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

                if (string.IsNullOrEmpty(silentFlag)) return;
                await context.RespondAsync($"Removed {futures.Count} 😉");
            } else await context.RespondAsync("You need to answer a message.");
        }

        [Command("deleten")]
        [Aliases("dn")]
        [Description("Deletes the n last messages.")]
        public async Task Delete(CommandContext context, int n)
        {
            if (n <= 0)
            {
                await context.RespondAsync($"{n} is not a valid number."); // todo cleanify
                return;
            }

            var limit = context.Message;
            var futures = context.Channel
                .GetMessagesBeforeAsync(limit.Id).Result
                .Take(n)
                .Select(msg => msg.DeleteAsync())
                .ToList();

            Task.WaitAll(futures.ToArray());
            await limit.DeleteAsync();
            await context.RespondAsync($"Removed {futures.Count} 😉");
        }
    }
}