using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("wind", "placeholder")]
public class WindCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("rots", "Adds a provided value to @someone’s rot score")]
    public async Task ScoreRot(InteractionContext context,
        [OptionAttribute("member", "User to increment the rot score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Wind, nb);
    }

    [SlashCommand("pets", "Adds a provided value to @someone’s pet score")]
    public async Task ScorePetWind(InteractionContext context,
        [OptionAttribute("member", "User to increment the pet score of")]
        DiscordUser member,
        [OptionAttribute("nb", "To increment by")]
        long nb)
    {
        await Controller.Score(context, member, CounterCategory.Wind, nb);
    }

    [SlashCommand("all", "Returns all wind scores")]
    public async Task ScoreWind(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Wind);
    }

    [SlashCommand("add", "Adds a record to @someone’s wind history and increments their score")]
    public async Task ScoreWind(InteractionContext context,
        [OptionAttribute("member", "User to increment the wind score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Wind, reason);
    }

    [SlashCommand("rot", "Adds a rot to @someone’s wind history and increments their score")]
    public async Task ScoreRot(InteractionContext context,
        [OptionAttribute("member", "User to increment the wind score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Wind, "(rot) " + reason);
    }

    [SlashCommand("pet", "Adds a pet to @someone’s wind history and increments their score")]
    public async Task ScorePet(InteractionContext context,
        [OptionAttribute("member", "User to increment the wind score of by 1")]
        DiscordUser member,
        [OptionAttribute("reason", "Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Wind, "(pet) " + reason);
    }
}