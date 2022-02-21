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
    public WeatherService Service { private get; set; } = null!;

    [GroupCommand]
    public async Task Demo(CommandContext context, [Description("City to know the weather of")] string city)
    {
        try
        {
            var current = await Service.CurrentFor(city);
            await context.RespondAsync(current.ToString());
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }
}