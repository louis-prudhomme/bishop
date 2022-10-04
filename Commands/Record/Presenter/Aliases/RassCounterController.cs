using System;
using System.Threading.Tasks;
using Bishop.Commands.Record.Model;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Presenter.Aliases;

public class RassCounterController : BaseCommandModule
{
    public Record.Presenter.RecordController Controller { private get; set; } = null!;

    [Command("rass")]
    [Description("Adds a provided value to @someone’s rass score")]
    public async Task ScoreRass(CommandContext context,
        [Description("User to increment the rass score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Controller.Score(context, member, CounterCategory.Rass, nb);
    }

    [Command("rass")]
    [Description("Returns the value of @someone’s rass score")]
    public async Task ScoreRass(CommandContext context,
        [Description("User to know the rass score of")]
        DiscordMember member)
    {
        await Controller.Score(context, member, CounterCategory.Rass);
    }

    [Command("rass")]
    [Description("Returns all rass scores")]
    public async Task ScoreRass(CommandContext context)
    {
        try
        {
            await Controller.Score(context, CounterCategory.Rass);
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }

    [Command("rass")]
    [Description("Adds a provided value to @someone’s rass score")]
    public async Task ScoreRass(CommandContext context,
        [Description("User to increment the rass score of by 1")]
        DiscordMember member,
        [RemainingText] [Description("Reason for the increment")]
        string reason)
    {
        await Controller.Score(context, member, CounterCategory.Rass, reason);
    }
}