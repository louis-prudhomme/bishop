using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller.Aliases;

public class SelCounterController : BaseCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [Command("sel")]
    [Description("Adds a provided value to @someone’s sel score")]
    public async Task ScoreSel(CommandContext context,
        [Description("User to increment the sel score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Controller.Score(context, member, CounterCategory.Sel, nb);
    }

    [Command("sel")]
    [Description("Returns the value of @someone’s sel score")]
    public async Task ScoreSel(CommandContext context,
        [Description("User to know the sel score of")]
        DiscordMember member)
    {
        await Controller.Score(context, member, CounterCategory.Sel);
    }

    [Command("sel")]
    [Description("Returns all sel scores")]
    public async Task ScoreSel(CommandContext context)
    {
        await Controller.Score(context, CounterCategory.Sel);
    }

    [Command("sel")]
    [Description("Adds a provided value to @someone’s sel score")]
    public async Task ScoreSel(CommandContext context,
        [Description("User to increment the sel score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Sel, reason);
    }
}