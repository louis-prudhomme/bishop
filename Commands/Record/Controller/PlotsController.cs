using System.Threading.Tasks;
using Bishop.Commands.Record.Business;
using Bishop.Commands.Record.Domain;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordController</c> class provides a set of commands to keep trace of user's
///     deeds.
///     This file contains all the general and generic commands.
/// </summary>
public partial class RecordController
{
    [SlashCommand("cumulative", "Draw a cumulative chart of someone's score over time")]
    public async Task Cumulative(InteractionContext context,
        [OptionAttribute("user", "Who to draw the chart for ?")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Wind", (int) CounterCategory.Wind)]
        CounterCategory category)
    {
        var records = await Manager.Find(user.Id, category);
        var graph = PlotManager.Cumulative(records);
        await SendGraph(context, graph);
    }

    [SlashCommand("histogram", "Draw a histogram chart of someone's score over time")]
    public async Task Histogram(InteractionContext context,
        [OptionAttribute("user", "Who to draw the chart for ?")]
        DiscordUser user,
        [OptionAttribute("category", "Which category ?")]
        [ChoiceName("category")]
        [Choice("BDM", (int) CounterCategory.Bdm)]
        [Choice("Beauf", (int) CounterCategory.Beauf)]
        [Choice("Malfoy", (int) CounterCategory.Malfoy)]
        [Choice("Raclette", (int) CounterCategory.Raclette)]
        [Choice("Rass", (int) CounterCategory.Rass)]
        [Choice("Sauce", (int) CounterCategory.Sauce)]
        [Choice("Sel", (int) CounterCategory.Sel)]
        [Choice("Wind", (int) CounterCategory.Wind)]
        CounterCategory category)
    {
        var records = await Manager.Find(user.Id, category);
        var graph = PlotManager.Histogram(records);
        await SendGraph(context, graph);
    }

    private async Task SendGraph(InteractionContext context, PlotImage image)
    {
        var builder = new DiscordMessageBuilder();
        await context.DeferAsync(true);

        using var file = image.Image();
        builder.AddFile(file.Stream());

        await context.EditResponseAsync(new DiscordWebhookBuilder(builder));
    }
}