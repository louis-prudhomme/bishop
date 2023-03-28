using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("sauce", "placeholder")]
public class SauceCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Adds a provided value to @someone’s sauce score")]
    public async Task ScoreSauce(InteractionContext context,
        [OptionAttribute("member", "User to increment the sauce score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Sauce, nb);
    }

    [SlashCommand("consult", "Returns the value of @someone’s sauce score")]
    public async Task ScoreSauce(InteractionContext context,
        [OptionAttribute("member", "User to know the sauce score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Sauce);
    }

    [SlashCommand("all", "Returns all sauce scores")]
    public async Task ScoreSauce(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Sauce);
    }

    [SlashCommand("add", "Adds a provided value to @someone’s sauce score")]
    public async Task ScoreSauce(InteractionContext context,
        [OptionAttribute("member", "User to increment the sauce score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Sauce, reason);
    }
}