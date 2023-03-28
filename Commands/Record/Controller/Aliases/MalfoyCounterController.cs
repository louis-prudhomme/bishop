using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("malfoy", "placeholder")]
public class MalfoyCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Adds a provided value to @someone’s malfoy score")]
    public async Task ScoreMalfoy(InteractionContext context,
        [OptionAttribute("member", "User to increment the malfoy score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Malfoy, nb);
    }

    [SlashCommand("consult", "Returns the value of @someone’s malfoy score")]
    public async Task ScoreMalfoy(InteractionContext context,
        [OptionAttribute("member", "User to know the malfoy score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Malfoy);
    }

    [SlashCommand("all", "Returns all malfoy scores")]
    public async Task ScoreMalfoy(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Malfoy);
    }

    [SlashCommand("add", "Adds a provided value to @someone’s malfoy score")]
    public async Task ScoreMalfoy(InteractionContext context,
        [OptionAttribute("member", "User to increment the malfoy score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Malfoy, reason);
    }
}