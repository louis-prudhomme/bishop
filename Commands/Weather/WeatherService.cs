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
        return WeatherBeaconsHolder.Types
            .Select(metric => (metric, currentWeather.Get(metric)))
            .Select(tuple => (tuple.metric, WeatherBeaconsHolder
                .GetTypeBeacon(tuple.metric)
                .Ratio(tuple.Item2)))
            .Select(tuple => $"{tuple.metric}: {tuple.Item2}%")
            .Aggregate((s, s1) => string.Join("\n", s, s1));
    }

    public async Task<string> CurrentMetrics(string city)
    {
        var currentWeather = await CurrentFor(city);
        return currentWeather.ToString();
    }
}