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
        switch(type)
        {
            case WeatherMetric.Cloud:
                return Cloud;
            case WeatherMetric.Day:
                return IsDay;
            case WeatherMetric.Humidity:
                return Humidity;
            case WeatherMetric.Rain:
                return Rain;
            case WeatherMetric.Temperature:
                return Temperature;
            case WeatherMetric.Wind:
                return Wind;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public override string ToString()
    {
        return $"Epoch: {DateHelper.FromTimestampToDateTime(Epoch)}\n" +
               $"Cloud: {Cloud} %\n" +
               $"IsDay: {IsDay}\n" +
               $"Humidity: {Humidity} %\n" +
               $"Rain: {Rain} mm\n" +
               $"Temperature: {Temperature} °C\n " +
               $"Wind: {Wind} km/h\n";
    }
}