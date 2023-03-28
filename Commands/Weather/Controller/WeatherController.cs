using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bishop.Commands.Weather.Domain;
using Bishop.Commands.Weather.Presenter;
using Bishop.Commands.Weather.Service;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Weather.Controller;

public class WeatherController : ApplicationCommandModule
{
    private static readonly TextInfo Capital = new CultureInfo("en-US", false).TextInfo;
    public WeatherService Service { private get; set; } = null!;

    [SlashCommand("weather", "Get the weather in a specific city")]
    public async Task Get(InteractionContext context,
        [OptionAttribute("city", "Which city ?")]
         string city)
    {
        if (string.IsNullOrEmpty(city)) return;
        try
        {
            var current = await Service.CurrentRatiosByMetrics(city);
            var weatherForecast = current
                .Select(tuple => WeatherFormatter
                    .CreateFor(tuple.Key, tuple.Value))
                .Select(formatter => formatter.ToString())
                .JoinWithNewlines();

            await context.CreateResponseAsync($"__Weather forecast for *{Capital.ToTitleCase(city)}* " +
                                       $"the *{DateHelper.FromDateTimeToStringDate(DateTime.Now)} at {DateHelper.FromDateTimeToStringTime(DateTime.Now)}*__" +
                                       $":\n{weatherForecast}");
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.BadRequest)
                await context.CreateResponseAsync($"No city was found with the name {city}");
        }
    }
}