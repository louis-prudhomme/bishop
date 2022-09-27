using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;

namespace Bishop.Commands.Weather;

public class WeatherService
{
    private const long CacheFor = 14400;

    private readonly Dictionary<string, WeatherEntity> _cache = new();

    public WeatherAccessor Accessor { private get; set; } = null!;

    public async Task<WeatherEntity> CurrentFor(string city)
    {
        var currentEpoch = DateHelper.FromDateTimeToTimestamp(DateTime.Now);
        var cityKey = city.Trim().ToLower();

        if (_cache.ContainsKey(cityKey))
        {
            var cachedEntity = _cache[cityKey];
            if (currentEpoch - cachedEntity.Epoch <= CacheFor) return _cache[cityKey];

            _cache.Remove(cityKey);
            _cache.Add(cityKey, await Accessor.Current(cityKey));
        }
        else
        {
            _cache.Add(cityKey, await Accessor.Current(cityKey));
        }

        return _cache[cityKey];
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
            .JoinWithNewlines();
    }

    public async Task<string> CurrentMetrics(string city)
    {
        var currentWeather = await CurrentFor(city);
        return currentWeather.ToString();
    }
}