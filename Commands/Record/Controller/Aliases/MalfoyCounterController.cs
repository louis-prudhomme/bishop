using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("malfoy", "Interfact with malfoy history")]
public class MalfoyCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Add many points to someone's malfoy history")]
    public async Task ScoreMalfoy(InteractionContext context,
        [OptionAttribute("user", "User to increment the malfoy score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Malfoy, nb);
    }

    [SlashCommand("consult", "Get someone’s malfoy score")]
    public async Task ScoreMalfoy(InteractionContext context,
        [OptionAttribute("user", "User to know the malfoy score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Malfoy);
    }

    [SlashCommand("all", "Get all malfoy scores")]
    public async Task ScoreMalfoy(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Malfoy);
    }

    [SlashCommand("add", "Add a record to someone's malfoy history")]
    public async Task ScoreMalfoy(InteractionContext context,
        [OptionAttribute("user", "User to increment the malfoy score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Malfoy, reason);
    }
}