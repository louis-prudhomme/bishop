using System.Threading.Tasks;
using Bishop.Commands.Weather;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace Bishop.Config.Converters;

/// <summary>
///     This classes allows conversion of a DSharp command string argument to a <c>Keys</c> enum value.
///     It allows transparent use of the <c>Keys</c> enum as parameters of theses functions.
/// </summary>
internal class WeatherMetricConverter : IArgumentConverter<WeatherMetric>
{
    public Task<Optional<WeatherMetric>> ConvertAsync(string value, CommandContext ctx)
    {
        return value.ToLower() switch
        {
            "cloud" => Task.FromResult(Optional.FromValue(WeatherMetric.Cloud)),
            "day" => Task.FromResult(Optional.FromValue(WeatherMetric.Day)),
            "humidity" => Task.FromResult(Optional.FromValue(WeatherMetric.Humidity)),
            "hum" => Task.FromResult(Optional.FromValue(WeatherMetric.Humidity)),
            "rain" => Task.FromResult(Optional.FromValue(WeatherMetric.Rain)),
            "temp" => Task.FromResult(Optional.FromValue(WeatherMetric.Temperature)),
            "temperature" => Task.FromResult(Optional.FromValue(WeatherMetric.Temperature)),
            "wind" => Task.FromResult(Optional.FromValue(WeatherMetric.Wind)),
            _ => Task.FromResult(Optional.FromNoValue<WeatherMetric>())
        };
    }
}