using System;
using System.Diagnostics;
using Bishop.Helper;

namespace Bishop.Commands.Weather;

public class WeatherEntity
{
    public long Epoch { get; set; }
    public float Temperature { get; set; }
    public float IsDay { get; set; }
    public float Wind { get; set; }
    public float Rain { get; set; }
    public float Humidity { get; set; }
    public float Cloud { get; set; }

    public float Get(WeatherMetric type)
    {
        return type switch
        {
            WeatherMetric.Cloud => Cloud,
            WeatherMetric.Day => IsDay,
            WeatherMetric.Humidity => Humidity,
            WeatherMetric.Rain => Rain,
            WeatherMetric.Temperature => Temperature,
            WeatherMetric.Wind => Wind,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public override string ToString()
    {
        return $"Epoch: {DateHelper.FromTimestampToDateTime(Epoch)}\n" +
               $"Clouds: {Cloud} %\n" +
               $"Day cycle: {IsDay}\n" +
               $"Humidity: {Humidity} %\n" +
               $"Rain: {Rain} mm\n" +
               $"Temperature: {Temperature} °C\n " +
               $"Wind: {Wind} km/h\n";
    }
}