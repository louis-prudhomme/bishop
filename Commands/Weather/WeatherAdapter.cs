namespace Bishop.Commands.Weather;

public class WeatherAdapter
{
    public static WeatherEntity FromDtoToEntity(WeatherDTO dto)
    {
        var entity = new WeatherEntity();
        var (location, current) = dto;
        entity.Cloud = current.cloud * 100;
        entity.IsDay = current.is_day;
        entity.Humidity = current.humidity;
        entity.Rain = current.precip_mm;
        entity.Temperature = current.temp_c;
        entity.Wind = current.wind_kph;
        entity.Epoch = location.localtime_epoch;
        return entity;
    }
}