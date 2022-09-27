namespace Bishop.Commands.Weather;

public class WeatherFormatter
{
    private readonly string _description;

    private readonly string _label;

    private WeatherFormatter(WeatherMetric metric, string description)
    {
        _description = description;
        _label = MetricToLabel(metric);
    }

    public static WeatherFormatter CreateFor(WeatherMetric metric, string description)
    {
        return new WeatherFormatter(metric, description);
    }

    public override string ToString()
    {
        return ToString(false);
    }

    public string ToString(bool isShort)
    {
        return isShort
            ? $" about {_label} : *{_description}*"
            : $" • {_label}: *{_description}*";
    }

    private static string MetricToLabel(WeatherMetric metric)
    {
        return metric switch
        {
            WeatherMetric.Cloud => "Cloud",
            WeatherMetric.Day => "Day",
            WeatherMetric.Humidity => "Humidity",
            WeatherMetric.Rain => "Rain",
            WeatherMetric.Temperature => "Temperature",
            WeatherMetric.Wind => "Wind",
            _ => "Suck it, bitch"
        };
    }
}