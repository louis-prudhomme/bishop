using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases;

public class MaufoiCounter : BaseCommandModule
{
    public CounterService Service { private get; set; } = null!;

    [Command("maufoi")]
    [Description("Adds a provided value to @someone’s maufoi score")]
    public async Task ScoreMaufoi(CommandContext context,
        [Description("User to increment the maufoi score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Maufoi, nb);
    }

    [Command("maufoi")]
    [Description("Returns the value of @someone’s maufoi score")]
    public async Task ScoreMaufoi(CommandContext context,
        [Description("User to know the maufoi score of")]
        DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Maufoi);
    }

    [Command("maufoi")]
    [Description("Returns all maufoi scores")]
    public async Task ScoreMaufoi(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Maufoi);
    }

    [Command("maufoi")]
    [Description("Adds a provided value to @someone’s maufoi score")]
    public async Task ScoreMaufoi(CommandContext context,
        [Description("User to increment the maufoi score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Maufoi, reason);
    }
}