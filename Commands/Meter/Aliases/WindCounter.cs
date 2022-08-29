using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases;

public class WindCounter : BaseCommandModule
{
    public CounterService Service { private get; set; } = null!;

    [Command("wind")]
    [Description("Adds a provided value to @someone’s wind score")]
    public async Task ScoreWind(CommandContext context,
        [Description("User to increment the wind score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Wind, nb);
    }

    [Command("wind")]
    [Description("Returns the value of @someone’s wind score")]
    public async Task ScoreWind(CommandContext context,
        [Description("User to know the wind score of")]
        DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Wind);
    }

    [Command("wind")]
    [Description("Returns all wind scores")]
    public async Task ScoreWind(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Wind);
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
        await Service.Score(context, member, CounterCategory.Wind, reason);
    }

    [Command("rot")]
    [Description("Adds a rot to @someone’s wind history and increments their score")]
    public async Task ScoreRot(CommandContext context,
        [Description("User to increment the wind score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Wind, "(rot) " + reason);
    }

    [Command("pet")]
    [Description("Adds a pet to @someone’s wind history and increments their score")]
    public async Task ScorePet(CommandContext context,
        [Description("User to increment the wind score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Wind, "(pet) " + reason);
    }
}