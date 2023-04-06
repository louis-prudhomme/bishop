using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("bdm", "Interact with BDM history")]
public class BdmCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Add many points to someone's BDM history")]
    public async Task ScoreBdm(InteractionContext context,
        [OptionAttribute("user", "User to increment the bdm score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Bdm, nb);
    }

    [SlashCommand("consult", "Get someone’s bdm score")]
    public async Task ScoreBdm(InteractionContext context,
        [OptionAttribute("user", "User to know the bdm score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Bdm);
    }

    [SlashCommand("podium", "Get all bdm scores")]
    public async Task ScoreBdm(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Bdm);
    }

    [SlashCommand("add", "Adds a record to someone’s bdm history and increments their score")]
    public async Task ScoreBdm(InteractionContext context,
        [OptionAttribute("user", "User to increment the bdm score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Bdm, reason);
    }
}
