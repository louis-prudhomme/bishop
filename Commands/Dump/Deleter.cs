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

    [SlashCommand("delete", "Deletes the N last messages.")]
    public async Task DeleteN(InteractionContext context,
        [OptionAttribute("Count", "How many messages ?")]
        [Maximum(MaxNumberOfDeletions)] [Minimum(1)] long n)
    {
        var n32 = Convert.ToInt32(n);

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