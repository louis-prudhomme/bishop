// ReSharper disable InconsistentNaming

namespace Bishop.Commands.Weather.Service;

public record WeatherDTO(Location location, Current current);

public record Location(long localtime_epoch);

public record Current(float temp_c, uint is_day, float wind_kph, float precip_mm, float humidity, float cloud);