using System;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller.Aliases;

public class RacletteCounterController : BaseCommandModule
{
    public RecordController Controller { private get; set; } = null!;

    [Command("raclette")]
    [Description("Gently tells off the user to add a date to the counter")]
    public async Task ScoreRaclette(CommandContext context,
        [Description("User to increment the raclette score of")]
        DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await context.RespondAsync("Where's the proof you fat cunt");
    }

    [Command("raclette")]
    [Description("Returns the value of @someone’s raclette score")]
    public async Task ScoreRaclette(CommandContext context,
        [Description("User to know the raclette score of")]
        DiscordMember member)
    {
        await Controller.Consult(context, member, CounterCategory.Raclette, null);
    }

    [Command("raclette")]
    [Description("Returns all raclette scores")]
    public async Task ScoreRaclette(CommandContext context)
    {
        await Controller.Score(context, CounterCategory.Raclette);
    }

    [Command("raclette")]
    [Description("Adds a provided value to @someone’s raclette score")]
    public async Task ScoreRaclette(CommandContext context,
        [Description("User to increment the raclette score of")]
        DiscordMember member,
        [Description("Date of the raclette")] string date)
    {
        long timestampedDate = DateHelper.FromDateTimeToTimestamp(DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture));
        if (timestampedDate != 0)
        {
            await Controller.Score(context, member, CounterCategory.Raclette, timestampedDate.ToString());
        }
    }
}