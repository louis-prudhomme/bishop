using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("beauf", "placeholder")]
public class BeaufCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Adds a provided value to @someone’s beauf score")]
    public async Task ScoreBeauf(InteractionContext context,
        [OptionAttribute("member", "User to increment the beauf score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Beauf, nb);
    }

    [SlashCommand("consult", "Returns the value of @someone’s beauf score")]
    public async Task ScoreBeauf(InteractionContext context,
        [OptionAttribute("member", "User to know the beauf score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Beauf);
    }

    [SlashCommand("all", "Returns all beauf scores")]
    public async Task ScoreBeauf(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Beauf);
    }

    [SlashCommand("add", "Adds a provided value to @someone’s beauf score")]
    public async Task ScoreBeauf(InteractionContext context,
        [OptionAttribute("member", "User to increment the beauf score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Beauf, reason);
    }
}