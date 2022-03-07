using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
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
    public async Task Get(CommandContext context)
    {
        await Get(context, "paris");
    }

    [GroupCommand]
    public async Task Get(CommandContext context, [Description("City to know the weather of")] string city)
    {
        try
        {
            var current = await Service.CurrentRatios(city);
            await context.RespondAsync(current.ToString());
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }
}