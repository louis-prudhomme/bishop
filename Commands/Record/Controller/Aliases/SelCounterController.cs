using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("sel", "Interact with sel history")]
public class SelCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Add many points to someone's sel history")]
    public async Task ScoreSel(InteractionContext context,
        [OptionAttribute("user", "User to increment the sel score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Sel, nb);
    }

    [SlashCommand("consult", "Get someone’s sel score")]
    public async Task ScoreSel(InteractionContext context,
        [OptionAttribute("user", "User to know the sel score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Sel);
    }

    [SlashCommand("all", "Get all sel scores")]
    public async Task ScoreSel(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Sel);
    }

    [SlashCommand("add", "Add a record to someone's sel history")]
    public async Task ScoreSel(InteractionContext context,
        [OptionAttribute("user", "User to increment the sel score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Sel, reason);
    }
}