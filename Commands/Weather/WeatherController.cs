﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Weather;

public class WeatherController : BaseCommandModule
{
    private static readonly TextInfo Capital = new CultureInfo("en-US", false).TextInfo;
    public WeatherService Service { private get; set; } = null!;

    [Command("weather")]
    [Aliases("w")]
    [Description("Gives a complete and accurate weather forecast of a city, as seen by a « parigot »")]
    public async Task Get(CommandContext context, [Description("City to know the weather of")] string city)
    {
        if (string.IsNullOrEmpty(city)) return;
        try
        {
            var current = await Service.CurrentRatiosByMetrics(city);
            var weatherForecast = current
                .Select(tuple => WeatherFormatter
                    .CreateFor(tuple.Key, tuple.Value))
                .Select(formatter => formatter.ToString())
                .Aggregate((line1, line2) => string.Join("\n", line1, line2));
            
            await context.RespondAsync($"__Weather forecast for *{Capital.ToTitleCase(city)}* " +
                                       $"at *{DateHelper.FromDateTimeToStringDate(DateTime.Now)} at {DateHelper.FromDateTimeToStringTime(DateTime.Now)}*__" +
                                       $":\n{weatherForecast}");
        }
        catch (HttpRequestException e)
        {
            var moncuq =  city.ToLower().Equals("moncuq") ? ", sale fils de pute" : "";
            if (e.StatusCode == HttpStatusCode.BadRequest)
                await context.RespondAsync($"No city was found with the name {city}{moncuq}");
        }
    }

    [Command("weather")]
    [Description("Displays a specific weather metric of a city at the moment, as seen by a « parigot »")]
    public async Task Get(CommandContext context, [Description("City to know the weather of")] string city,
        [Description("Metric to learn about")] WeatherMetric metric)
    {
        if (string.IsNullOrEmpty(city)) return;
        try
        {
            var current = await Service.CurrentRatiosByMetrics(city);
            await context.RespondAsync($"__Currently in *{Capital.ToTitleCase(city)}* :__ {WeatherFormatter.CreateFor(metric, current[metric]).ToString(true)}");
        }
        catch (HttpRequestException e)
        {
            var moncuq =  city.ToLower().Equals("moncuq") ? ", sale fils de pute" : "";
            if (e.StatusCode == HttpStatusCode.BadRequest)
                await context.RespondAsync($"No city was found with the name « {city} »{moncuq}");
        }
    }
}