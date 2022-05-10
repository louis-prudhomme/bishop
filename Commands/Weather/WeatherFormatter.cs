namespace Bishop.Commands.Weather;

public class WeatherFormatter
{
    public static WeatherFormatter CreateFor(WeatherMetric metric, string description)
    {
        return new WeatherFormatter(metric, description);
    }

    private readonly string _label;
    private readonly string _description;

    private WeatherFormatter(WeatherMetric metric, string description)
    {
        _description = description;
        _label = MetricToLabel(metric);
    }

    public override string ToString() => ToString(false);
    
    public string ToString(bool isShort)
    {
        return isShort
            ? $" about {_label} - *{_description}*"
            : $" • {_label}: *{_description}*";
    }

    private static string MetricToLabel(WeatherMetric metric) => metric switch
    {
        WeatherMetric.Cloud => "Cloud",
        WeatherMetric.Day => "Day",
        WeatherMetric.Humidity => "Humidity",
        WeatherMetric.Rain => "Rain",
        WeatherMetric.Temperature => "Temperature",
        WeatherMetric.Wind => "Wind"
    };
}