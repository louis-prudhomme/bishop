using System;
using System.Collections.Generic;
using System.Linq;
using Bishop.Helper;

namespace Bishop.Commands.Weather.Domain;

public class WeatherBeaconsHolder
{
    private const string WeatherFilePath = "weatherBeacons.json";

    public static readonly List<WeatherMetric> Types = new(Enum.GetValues(typeof(WeatherMetric)).Cast<WeatherMetric>());

    private static readonly List<WeatherBeaconEntity> Beacons =
        new JsonDeserializer<List<WeatherBeaconEntity>>(WeatherFilePath)
            .Get().Result;

    public static WeatherBeaconEntity GetTypeBeacon(WeatherMetric type)
    {
        return Beacons.First(entity => entity.Type == type);
    }
}