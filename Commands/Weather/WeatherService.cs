using System;
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

    public async Task<string> CurrentRatios(string city)
    {
        var currentWeather = await CurrentFor(city);
        List<float> ratios = new()
        {
            WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Cloud).Ratio(currentWeather.Cloud),
            WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Day).Ratio(currentWeather.IsDay),
            WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Humidity).Ratio(currentWeather.Humidity),
            WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Rain).Ratio(currentWeather.Rain),
            WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Temperature).Ratio(currentWeather.Temperature),
            WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Wind).Ratio(currentWeather.Wind)
        };
        var beacon = WeatherBeaconsHolder.GetTypeBeacon(WeatherMetric.Rain);
        var ratio = $"{beacon.Type}: {currentWeather.Rain} vs {beacon.Max}/{beacon.Min} = {beacon.Ratio(currentWeather.Rain)}";
        
        return string.Join("\n", ratio);
    }
}