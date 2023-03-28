using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Exceptions;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

/// <summary>
///     This class provides a set of commands to delete messages.
/// </summary>
internal class Deleter : ApplicationCommandModule
{
    private const int MaxNumberOfDeletions = 100;

    [SlashCommand("delete", "Deletes all the messages between the command and the one replied to.")]
    public async Task Delete(InteractionContext context, [OptionAttribute("Silence", "Must I be silent?")] string silentFlag)
    {
        //FIXME: i do not work as of right now
        if (false)
        {
            var messagesToDelete = (await context.Channel
                    .GetMessagesAfterAsync(context.InteractionId))
                .TakeWhile(msg => msg.Timestamp >= context.Interaction.CreationTimestamp)
                .ToList();

            if (messagesToDelete.Count == MaxNumberOfDeletions)
            {
                //TODO find a way to bulk delete even hen more than a hundred (name it bulk nuke?)
                await context.CreateResponseAsync("There are more than a hundred messages, cannot delete.");
                return;
            }

            await context.Channel.DeleteMessagesAsync(messagesToDelete);

            if (!string.IsNullOrEmpty(silentFlag)) return;
            await context.CreateResponseAsync($"Removed {messagesToDelete.Count} 😉");
        }
        else await context.CreateResponseAsync("You need to answer a message.");
    }

    [SlashCommand("deleten", "Deletes the n last messages.")]
    public async Task DeleteN(InteractionContext context, [OptionAttribute("Count", "How many messages to delete")] long n)
    {
        var n32 = Convert.ToInt32(n);

        if (n >= MaxNumberOfDeletions)
        {
            //TODO find a way to bulk delete even hen more than a hundred (name it bulk nuke?)
            await context.CreateResponseAsync("There are more than a hundred messages, cannot delete.");
            return;
        }

        var messagesToDelete = (await context.Channel
                .GetMessagesBeforeAsync(context.InteractionId, n32))
            .Take(n32)
            .ToList();

        try
        {
            await context.Channel.DeleteMessagesAsync(messagesToDelete);

            await context.CreateResponseAsync($"Removed {messagesToDelete.Count} 😉");
        }
        catch (BadRequestException)
        {
            await context.CreateResponseAsync("Messages more than 14 days old cannot be deleted through my services.");
        }
    }
}