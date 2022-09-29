using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.History.Aliases;

public class BeaufCounter : BaseCommandModule
{
    public CounterService Service { private get; set; } = null!;

    [Command("beauf")]
    [Description("Adds a provided value to @someone’s beauf score")]
    public async Task ScoreBeauf(CommandContext context,
        [Description("User to increment the beauf score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Beauf, nb);
    }

    [Command("beauf")]
    [Description("Returns the value of @someone’s beauf score")]
    public async Task ScoreBeauf(CommandContext context,
        [Description("User to know the beauf score of")]
        DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Beauf);
    }

    [Command("beauf")]
    [Description("Returns all beauf scores")]
    public async Task ScoreBeauf(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Beauf);
    }

    [Command("beauf")]
    [Description("Adds a provided value to @someone’s beauf score")]
    public async Task ScoreBeauf(CommandContext context,
        [Description("User to increment the beauf score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Service.Score(context, member, CounterCategory.Beauf, reason);
    }
}