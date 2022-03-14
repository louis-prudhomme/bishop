using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;

namespace Bishop.Commands.Weather;

public class WeatherService
{
    public const long CacheFor = 14400;

    public Dictionary<string, WeatherEntity> Cache = new();

    public List<WeatherMetric> types =
        new List<WeatherMetric>(Enum.GetValues(typeof(WeatherMetric)).Cast<WeatherMetric>());

    public WeatherAccessor Accessor { private get; set; } = null!;

    public async Task<WeatherEntity> CurrentFor(string city)
    {
        var currentEpoch = DateHelper.FromDateTimeToTimestamp(DateTime.Now);
        var cityKey = city.Trim().ToLower();

        if (Cache.ContainsKey(cityKey))
        {
            var cachedEntity = Cache[cityKey];
            if (currentEpoch - cachedEntity.Epoch <= CacheFor) return Cache[cityKey];

            Cache.Remove(cityKey);
            Cache.Add(cityKey, await Accessor.Current(cityKey));
        }
        else
        {
            Cache.Add(cityKey, await Accessor.Current(cityKey));
        }

        return Cache[cityKey];
    }

    public async Task<string> CurrentRatios(string city)
    {
        var currentWeather = await CurrentFor(city);
        var ratios = types
            .Select(type => WeatherBeaconsHolder.GetTypeBeacon(type)
                .Ratio(currentWeather.Get(type)))
            .ToList();

        return string.Join("\n", ratios);
    }

    public async Task<float> CurrentRatio(string city, WeatherMetric metric)
    {
        var currentWeather = await CurrentFor(city);
        return WeatherBeaconsHolder
            .GetTypeBeacon(metric)
            .Ratio(currentWeather
                .Get(metric));
    }
}