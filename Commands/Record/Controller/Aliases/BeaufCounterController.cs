using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("beauf", "Interact with beauf history")]
public class BeaufCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("addmany", "Add many points to someone's beauf history")]
    public async Task ScoreBeauf(InteractionContext context,
        [OptionAttribute("user", "User to increment the beauf score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Beauf, nb);
    }

    [SlashCommand("consult", "Get someone’s beauf score")]
    public async Task ScoreBeauf(InteractionContext context,
        [OptionAttribute("user", "User to know the beauf score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Beauf);
    }

    [SlashCommand("podium", "Get all beauf scores")]
    public async Task ScoreBeauf(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Beauf);
    }

    [SlashCommand("add", "Add a record to someone's beauf history")]
    public async Task ScoreBeauf(InteractionContext context,
        [OptionAttribute("user", "User to increment the beauf score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Beauf, reason);
    }
}
