using Bishop.Helper;

namespace Bishop.Commands.Weather;

public class WeatherEntity
{
    public long Epoch { get; set; }
    public float Temperature { get; set; }
    public bool IsDay { get; set; }
    public float Wind { get; set; }
    public float Precipitation { get; set; }
    public float Humidity { get; set; }
    public float Cloud { get; set; }

    public override string ToString()
    {
        return $"Epoch: {DateHelper.FromTimestampToDateTime(Epoch)}\n" +
               $"Temperature: {Temperature} °C\n " +
               $"IsDay: {IsDay}\n" +
               $"Wind: {Wind} km/h\n" +
               $"Precipitation: {Precipitation} mm\n" +
               $"Humidity: {Humidity} %\n" +
               $"Cloud: {Cloud} %\n";
    }
}