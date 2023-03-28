using System;
using System.Globalization;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("raclette", "placeholder")]
public class RacletteCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("consult", "Returns the value of @someone’s raclette score")]
    public async Task ScoreRaclette(InteractionContext context,
        [OptionAttribute("member", "User to know the raclette score of")]
        DiscordUser member)
    {
        await Controller.Consult(context, member, CounterCategory.Raclette);
    }

    [SlashCommand("all", "Returns all raclette scores")]
    public async Task ScoreRaclette(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Raclette);
    }

    [SlashCommand("add", "Adds a provided value to @someone’s raclette score")]
    public async Task ScoreRaclette(InteractionContext context,
        [OptionAttribute("member", "User to increment the raclette score of")]
        DiscordUser member,
        [OptionAttribute("date", "Date of the raclette")]
        string date)
    {
        var timestampedDate = DateHelper.FromDateTimeToTimestamp(DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture));
        if (timestampedDate != 0) await Controller.Score(context, member, CounterCategory.Raclette, timestampedDate.ToString());
    }
}