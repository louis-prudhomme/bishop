using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("wind", "Interact with wind history")]
public class WindCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("rots", "Add a record to someone's rot history")]
    public async Task ScoreRot(InteractionContext context,
        [OptionAttribute("user", "User to increment the rot score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Wind, nb);
    }

    [SlashCommand("pets", "Add a record to someone's pet history")]
    public async Task ScorePetWind(InteractionContext context,
        [OptionAttribute("user", "User to increment the pet score of")]
        DiscordUser user,
        [OptionAttribute("points", "How many points ?")]
        [Maximum(10)]
        [Minimum(1)]
        long nb)
    {
        await Controller.Score(context, user, CounterCategory.Wind, nb);
    }

    [SlashCommand("podium", "Get all wind scores")]
    public async Task ScoreWind(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Wind);
    }

    [SlashCommand("add", "Adds a record to someone’s wind history and increments their score")]
    public async Task ScoreWind(InteractionContext context,
        [OptionAttribute("user", "User to increment the wind score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Wind, reason);
    }

    [SlashCommand("rot", "Adds a rot to someone’s wind history and increments their score")]
    public async Task ScoreRot(InteractionContext context,
        [OptionAttribute("user", "User to increment the wind score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Wind, "(rot) " + reason);
    }

    [SlashCommand("pet", "Adds a pet to someone’s wind history and increments their score")]
    public async Task ScorePet(InteractionContext context,
        [OptionAttribute("user", "User to increment the wind score of by 1")]
        DiscordUser user,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, user, CounterCategory.Wind, "(pet) " + reason);
    }
}
