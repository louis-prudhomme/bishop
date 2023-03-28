using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("sauce", "Interact with sauce history")]
public class SauceCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Add many points to someone's sauce history")]
    public async Task ScoreSauce(InteractionContext context,
        [OptionAttribute("user", "User to increment the sauce score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Sauce, nb);
    }

    [SlashCommand("consult", "Get someone’s sauce score")]
    public async Task ScoreSauce(InteractionContext context,
        [OptionAttribute("user", "User to know the sauce score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Sauce);
    }

    [SlashCommand("all", "Get all sauce scores")]
    public async Task ScoreSauce(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Sauce);
    }

    [SlashCommand("add", "Add a record to someone's sauce history")]
    public async Task ScoreSauce(InteractionContext context,
        [OptionAttribute("user", "User to increment the sauce score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Sauce, reason);
    }
}