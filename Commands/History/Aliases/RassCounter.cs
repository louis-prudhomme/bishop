using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.History.Aliases;

public class RassCounter : BaseCommandModule
{
    public RecordService Service { private get; set; } = null!;

    [Command("rass")]
    [Description("Adds a provided value to @someone’s rass score")]
    public async Task ScoreRass(CommandContext context,
        [Description("User to increment the rass score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Rass, nb);
    }

    [Command("rass")]
    [Description("Returns the value of @someone’s rass score")]
    public async Task ScoreRass(CommandContext context,
        [Description("User to know the rass score of")]
        DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Rass);
    }

    [Command("rass")]
    [Description("Returns all rass scores")]
    public async Task ScoreRass(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Rass);
    }

    [Command("rass")]
    [Description("Adds a provided value to @someone’s rass score")]
    public async Task ScoreRass(CommandContext context,
        [Description("User to increment the rass score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Rass, reason);
    }
}