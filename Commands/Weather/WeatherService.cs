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

    public async Task<Dictionary<WeatherMetric, string>> CurrentRatiosByMetrics(string city)
    {
        var currentWeather = await CurrentFor(city);
        return WeatherBeaconsHolder.Types
            .Select(type => (type, WeatherBeaconsHolder.GetTypeBeacon(type)
                .Ratio(currentWeather.Get(type)) * 100))
            .Select(tuple => (tuple.type, WeatherBeaconsHolder
                .GetTypeBeacon(tuple.type)
                .LevelFor(tuple.Item2)))
            .ToDictionary(tuple => tuple.type, tuple => tuple.Item2);
    }

    public async Task<string> CurrentRatios(string city)
    {
        var currentWeather = await CurrentFor(city);
        var ratios = WeatherBeaconsHolder.Types
            .Select(type => WeatherBeaconsHolder.GetTypeBeacon(type)
                .Ratio(currentWeather.Get(type)) * 100)
            .ToList();

        return string.Join("\n", ratios);
    }

    public async Task<string> CurrentRatio(string city, WeatherMetric metric)
    {
        var currentWeather = await CurrentFor(city);
        var beacon = WeatherBeaconsHolder
            .GetTypeBeacon(metric);
        var ratio = beacon.Ratio(currentWeather.Get(metric));
        var level = beacon.LevelFor(ratio * 100);

        return level;
    }
}