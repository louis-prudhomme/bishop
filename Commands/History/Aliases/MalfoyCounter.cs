using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.History.Aliases;

public class MalfoyCounter : BaseCommandModule
{
    public RecordService Service { private get; set; } = null!;

    [Command("malfoy")]
    [Description("Adds a provided value to @someone’s malfoy score")]
    public async Task ScoreMalfoy(CommandContext context,
        [Description("User to increment the malfoy score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Malfoy, nb);
    }

    [Command("malfoy")]
    [Description("Returns the value of @someone’s malfoy score")]
    public async Task ScoreMalfoy(CommandContext context,
        [Description("User to know the malfoy score of")]
        DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Malfoy);
    }

    [Command("malfoy")]
    [Description("Returns all malfoy scores")]
    public async Task ScoreMalfoy(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Malfoy);
    }

    [Command("malfoy")]
    [Description("Adds a provided value to @someone’s malfoy score")]
    public async Task ScoreMalfoy(CommandContext context,
        [Description("User to increment the malfoy score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Malfoy, reason);
    }
}