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