using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller.Aliases;

public class WindCounterController : BaseCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [Command("wind")]
    [Description("Adds a provided value to @someone’s wind score")]
    public async Task ScoreWind(CommandContext context,
        [Description("User to increment the wind score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Controller.Score(context, member, CounterCategory.Wind, nb);
    }

    [Command("rot")]
    [Description("Adds a provided value to @someone’s rot score")]
    public async Task ScoreRot(CommandContext context,
        [Description("User to increment the rot score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Controller.Score(context, member, CounterCategory.Wind, nb);
    }

    [Command("pet")]
    [Description("Adds a provided value to @someone’s pet score")]
    public async Task ScorePetWind(CommandContext context,
        [Description("User to increment the pet score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Controller.Score(context, member, CounterCategory.Wind, nb);
    }

    [Command("wind")]
    [Description("Returns the value of @someone’s wind score")]
    public async Task ScoreWind(CommandContext context,
        [Description("User to know the wind score of")]
        DiscordMember member)
    {
        await Controller.Score(context, member, CounterCategory.Wind);
    }

    [Command("wind")]
    [Description("Returns all wind scores")]
    public async Task ScoreWind(CommandContext context)
    {
        await Controller.Score(context, CounterCategory.Wind);
    }

    [Command("rot")]
    [Description("Returns all wind scores")]
    public async Task ScoreRot(CommandContext context)
    {
        await ScoreWind(context);
    }

    [Command("pet")]
    [Description("Returns all wind scores")]
    public async Task ScorePet(CommandContext context)
    {
        await ScoreWind(context);
    }

    [Command("wind")]
    [Description("Adds a record to @someone’s wind history and increments their score")]
    public async Task ScoreWind(CommandContext context,
        [Description("User to increment the wind score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Wind, reason);
    }

    [Command("rot")]
    [Description("Adds a rot to @someone’s wind history and increments their score")]
    public async Task ScoreRot(CommandContext context,
        [Description("User to increment the wind score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Wind, "(rot) " + reason);
    }

    [Command("pet")]
    [Description("Adds a pet to @someone’s wind history and increments their score")]
    public async Task ScorePet(CommandContext context,
        [Description("User to increment the wind score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Wind, "(pet) " + reason);
    }
}