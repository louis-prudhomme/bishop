using System;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("rass", "Interact with rass history")]
public class RassCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Add many points to someone's rass history")]
    public async Task ScoreRass(InteractionContext context,
        [OptionAttribute("user", "User to increment the rass score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Rass, nb);
    }

    [SlashCommand("consult", "Get someone’s rass score")]
    public async Task ScoreRass(InteractionContext context,
        [OptionAttribute("user", "User to know the rass score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Rass);
    }

    [SlashCommand("podium", "Get all rass scores")]
    public async Task ScoreRass(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Rass);
    }

    [SlashCommand("add", "Add a record to someone's rass history")]
    public async Task ScoreRass(InteractionContext context,
        [OptionAttribute("user", "User to increment the rass score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Rass, reason);
    }
}
