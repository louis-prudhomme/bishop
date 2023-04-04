using System;
using System.Globalization;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Record.Controller.Aliases;

[SlashCommandGroup("raclette", "Interact with raclette history")]
public class RacletteCounterController : ApplicationCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [SlashCommand("consult", "Get someone’s raclette score")]
    public async Task ScoreRaclette(InteractionContext context,
        [OptionAttribute("user", "User to know the raclette score of")]
        DiscordUser user)
    {
        await Controller.Consult(context, user, CounterCategory.Raclette);
    }

    [SlashCommand("podium", "Get all raclette scores")]
    public async Task ScoreRaclette(InteractionContext context)
    {
        await Controller.Score(context, CounterCategory.Raclette);
    }

    [SlashCommand("add", "Add a record to someone's raclette history")]
    public async Task ScoreRaclette(InteractionContext context,
        [OptionAttribute("user", "User to increment the raclette score of")]
        DiscordUser user,
        [OptionAttribute("date", "Date of the raclette")]
        string date)
    {
        var timestampedDate = DateHelper.FromDateTimeToTimestamp(DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture));
        if (timestampedDate != 0) await Controller.Score(context, user, CounterCategory.Raclette, timestampedDate.ToString());
    }
}
