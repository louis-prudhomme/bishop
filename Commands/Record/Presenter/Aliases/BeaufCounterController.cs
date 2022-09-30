using System.Threading.Tasks;
using Bishop.Commands.Record.Model;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Presenter.Aliases;

public class BeaufCounterController : BaseCommandModule
{
    public Record.Presenter.RecordController Controller { private get; set; } = null!;

    [Command("beauf")]
    [Description("Adds a provided value to @someone’s beauf score")]
    public async Task ScoreBeauf(CommandContext context,
        [Description("User to increment the beauf score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Controller.Score(context, member, CounterCategory.Beauf, nb);
    }

    [Command("beauf")]
    [Description("Returns the value of @someone’s beauf score")]
    public async Task ScoreBeauf(CommandContext context,
        [Description("User to know the beauf score of")]
        DiscordMember member)
    {
        await Controller.Score(context, member, CounterCategory.Beauf);
    }

    [Command("beauf")]
    [Description("Returns all beauf scores")]
    public async Task ScoreBeauf(CommandContext context)
    {
        await Controller.Score(context, CounterCategory.Beauf);
    }

    [Command("beauf")]
    [Description("Adds a provided value to @someone’s beauf score")]
    public async Task ScoreBeauf(CommandContext context,
        [Description("User to increment the beauf score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Beauf, reason);
    }
}