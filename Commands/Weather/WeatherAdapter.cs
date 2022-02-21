namespace Bishop.Commands.Weather;

public class WeatherAdapter
{
    public static WeatherEntity FromDtoToEntity(WeatherDTO dto)
    {
        var entity = new WeatherEntity();
        var (location, current) = dto;
        entity.Cloud = current.cloud;
        entity.Humidity = current.humidity;
        entity.Precipitation = current.precip_mm;
        entity.Temperature = current.temp_c;
        entity.IsDay = current.is_day == 1;
        entity.Wind = current.wind_kph;
        entity.Epoch = location.localtime_epoch;
        return entity;
    }
}