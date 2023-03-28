using System;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("rass", "placeholder")]
public class RassCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Adds a provided value to @someone’s rass score")]
    public async Task ScoreRass(InteractionContext context,
        [OptionAttribute("member", "User to increment the rass score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Rass, nb);
    }

    [SlashCommand("consult", "Returns the value of @someone’s rass score")]
    public async Task ScoreRass(InteractionContext context,
        [OptionAttribute("member", "User to know the rass score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Rass);
    }

    [SlashCommand("all", "Returns all rass scores")]
    public async Task ScoreRass(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Rass);
    }

    [SlashCommand("add", "Adds a provided value to @someone’s rass score")]
    public async Task ScoreRass(InteractionContext context,
        [OptionAttribute("member", "User to increment the rass score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Rass, reason);
    }
}