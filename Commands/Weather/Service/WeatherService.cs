using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Weather.Domain;
using Bishop.Helper;

namespace Bishop.Commands.Weather.Service;

public class WeatherService
{

    public IKeyBasedCache<string, WeatherEntity> Cache { private get; set; } = null!;
    
    public WeatherEntity CurrentFor(string city)
    {
        var cityKey = city.Trim().ToLower();

        // TODO fix me
        return Cache.Get(cityKey).Value!;
    }

    public Dictionary<WeatherMetric, string> CurrentRatiosByMetrics(string city)
    {
        var currentWeather = CurrentFor(city);
        return WeatherBeaconsHolder.Types
            .Select(type => (type, WeatherBeaconsHolder.GetTypeBeacon(type)
                .Ratio(currentWeather.Get(type)) * 100))
            .Select(tuple => (tuple.type, WeatherBeaconsHolder
                .GetTypeBeacon(tuple.type)
                .LevelFor(tuple.Item2)))
            .ToDictionary(tuple => tuple.type, tuple => tuple.Item2);
    }

    public string CurrentRatios(string city)
    {
        var currentWeather = CurrentFor(city);
        return WeatherBeaconsHolder.Types
            .Select(metric => (metric, currentWeather.Get(metric)))
            .Select(tuple => (tuple.metric, WeatherBeaconsHolder
                .GetTypeBeacon(tuple.metric)
                .Ratio(tuple.Item2)))
            .Select(tuple => $"{tuple.metric}: {tuple.Item2}%")
            .JoinWithNewlines();
    }

    public string CurrentMetrics(string city)
    {
        var currentWeather = CurrentFor(city);
        return currentWeather.ToString();
    }
}