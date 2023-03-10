using System.Threading.Tasks;
using Bishop.Commands.Record.Business;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordController</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
/// </summary>
public partial class RecordController
{
    [Command("cumulative")]
    public async Task Cumulative(CommandContext context, DiscordMember member, CounterCategory category)
    {
        var records = await Manager.Find(member.Id, category);
        var graph = PlotManager.Cumulative(records);
        await SendGraph(context, graph);
    }

    [Command("histogram")]
    public async Task Histogram(CommandContext context, DiscordMember member, CounterCategory category)
    {
        var records = await Manager.Find(member.Id, category);
        var graph = PlotManager.Histogram(records);
        await SendGraph(context, graph);
    }

    private async Task SendGraph(CommandContext context, PlotImage image)
    {
        var builder = new DiscordMessageBuilder();
        using var file = image.Image();
        var temp = await context.RespondAsync("Sending...");
        builder.WithFile(file.Stream());
        await context.RespondAsync(builder);
        await temp.DeleteAsync();
    }
}