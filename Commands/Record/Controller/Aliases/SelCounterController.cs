using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("sel", "placeholder")]
public class SelCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Adds a provided value to @someone’s sel score")]
    public async Task ScoreSel(InteractionContext context,
        [OptionAttribute("member", "User to increment the sel score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Sel, nb);
    }

    [SlashCommand("consult", "Returns the value of @someone’s sel score")]
    public async Task ScoreSel(InteractionContext context,
        [OptionAttribute("member", "User to know the sel score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Sel);
    }

    [SlashCommand("all", "Returns all sel scores")]
    public async Task ScoreSel(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Sel);
    }

    [SlashCommand("add", "Adds a provided value to @someone’s sel score")]
    public async Task ScoreSel(InteractionContext context,
        [OptionAttribute("member", "User to increment the sel score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Sel, reason);
    }
}