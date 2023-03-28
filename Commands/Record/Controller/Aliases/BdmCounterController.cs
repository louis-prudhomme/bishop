using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("bdm", "placeholder")]
public class BdmCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Adds a provided value to @someone’s bdm score")]
    public async Task ScoreBdm(InteractionContext context,
        [OptionAttribute("member", "User to increment the bdm score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Bdm, nb);
    }

    [SlashCommand("consult", "Returns the value of @someone’s bdm score")]
    public async Task ScoreBdm(InteractionContext context,
        [OptionAttribute("member", "User to know the bdm score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Bdm);
    }

    [SlashCommand("all", "Returns all bdm scores")]
    public async Task ScoreBdm(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Bdm);
    }

    [SlashCommand("add", "Adds a record to @someone’s bdm history and increments their score")]
    public async Task ScoreBdm(InteractionContext context,
        [OptionAttribute("member", "User to increment the bdm score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Bdm, reason);
    }
}