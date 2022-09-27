using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases;

public class RacletteCounter : BaseCommandModule
{
    public CounterService Service { private get; set; } = null!;

    [Command("raclette")]
    [Description("Adds a provided value to @someone’s raclette score")]
    public async Task ScoreRaclette(CommandContext context,
        [Description("User to increment the raclette score of")]
    DiscordMember member,
        [Description("To increment by")] long nb)
    {
        await Service.Score(context, member, CounterCategory.Raclette, nb);
    }

    [Command("raclette")]
    [Description("Returns the value of @someone’s raclette score")]
    public async Task ScoreRaclette(CommandContext context,
        [Description("User to know the raclette score of")]
    DiscordMember member)
    {
        await Service.Score(context, member, CounterCategory.Raclette);
    }

    [Command("raclette")]
    [Description("Returns all raclette scores")]
    public async Task ScoreRaclette(CommandContext context)
    {
        await Service.Score(context, CounterCategory.Raclette);
    }
}
