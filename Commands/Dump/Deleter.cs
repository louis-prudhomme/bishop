using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

/// <summary>
///     This class provides a set of commands to delete messages.
/// </summary>
internal class Deleter : BaseCommandModule
{
    private static readonly int MAX_NUMBER_OF_DELETIONS = 100;

    [Command("delete")]
    [Aliases("d")]
    [Description("Deletes all the messages between the command and the one replied to.")]
    public async Task Delete(CommandContext context, [RemainingText] string silentFlag)
    {
        if (context.Message.MessageType == MessageType.Reply)
        {
            var limit = context.Message;
            var origin = limit.ReferencedMessage;

            var messagesToDelete = (await context.Channel
                    .GetMessagesAfterAsync(origin.Id))
                .TakeWhile(msg => msg.Timestamp >= origin.Timestamp)
                    .ToList();

                if (messagesToDelete.Count == MAX_NUMBER_OF_DELETIONS)
                {
                    //TODO find a way to bulk delete even hen more than a hundred (name it bulk nuke?)
                    await context.RespondAsync($"There are more than a hundred messages, cannot delete.");
                    return;
                }
            
            await context.Channel.DeleteMessagesAsync(messagesToDelete);

            if (!string.IsNullOrEmpty(silentFlag)) return;
            await context.RespondAsync($"Removed {messagesToDelete.Count} 😉");
        }
        else await context.RespondAsync("You need to answer a message.");
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
        
        if (n >= MAX_NUMBER_OF_DELETIONS)
        {
            //TODO find a way to bulk delete even hen more than a hundred (name it bulk nuke?)
            await context.RespondAsync($"There are more than a hundred messages, cannot delete.");
            return;
        }

        var limit = context.Message;
        var messagesToDelete = (await context.Channel
                .GetMessagesBeforeAsync(limit.Id))
            .Take(n)
            .ToList();


        await context.Channel.DeleteMessagesAsync(messagesToDelete);
        await limit.DeleteAsync();

        await context.RespondAsync($"Removed {messagesToDelete.Count} 😉");
    }
}