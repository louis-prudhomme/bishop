using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Weather;

[Group("weather")]
[Aliases("w")]
[Description("Weather-related commands")]
public class WeatherController : BaseCommandModule
{
    public WeatherAccessor Accessor { private get; set; } = null!;

    [GroupCommand]
    public async Task Demo(CommandContext context)
    {
        try
        {
            var current = await Accessor.Current();
            await context.RespondAsync(current.ToString());
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }
}