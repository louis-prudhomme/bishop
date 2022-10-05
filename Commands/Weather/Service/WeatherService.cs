using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Weather.Domain;
using Bishop.Helper;

namespace Bishop.Commands.Weather.Service;

public class WeatherService
{

    public IKeyBasedCache<string, WeatherEntity> Cache { private get; set; } = null!;

    private async Task<WeatherEntity?> CurrentFor(string city)
    {
        var cityKey = city.Trim().ToLower();

        return await Cache.GetValue(cityKey);
    }

    public async Task<Dictionary<WeatherMetric, string>> CurrentRatiosByMetrics(string city)
    {
        var currentWeather = await CurrentFor(city);
        if (currentWeather == null) return new Dictionary<WeatherMetric, string>();
        
        return WeatherBeaconsHolder.Types
            .Select(type => (type, WeatherBeaconsHolder.GetTypeBeacon(type)
                .Ratio(currentWeather.Get(type)) * 100))
            .Select(tuple => (tuple.type, WeatherBeaconsHolder
                .GetTypeBeacon(tuple.type)
                .LevelFor(tuple.Item2)))
            .ToDictionary(tuple => tuple.type, tuple => tuple.Item2);
    }
}