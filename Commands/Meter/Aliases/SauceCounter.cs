using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases;

public class SauceCounter : BaseCommandModule
{
    public CounterService Service { private get; set; } = null!;

    [Command("sauce")]
    [Description("Adds a provided value to @someone’s sauce score")]
    public async Task ScoreSauce(CommandContext context,
        [Description("User to increment the sauce score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Sauce, nb);
    }

    [Command("sauce")]
    [Description("Returns the value of @someone’s sauce score")]
    public async Task ScoreSauce(CommandContext context,
        [Description("User to know the sauce score of")]
        DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Sauce);
    }

    [Command("sauce")]
    [Description("Returns all sauce scores")]
    public async Task ScoreSauce(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Sauce);
    }

    [Command("sauce")]
    [Description("Adds a provided value to @someone’s sauce score")]
    public async Task ScoreSauce(CommandContext context,
        [Description("User to increment the sauce score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Sauce, reason);
    }
}