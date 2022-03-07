using System.Collections.Generic;

namespace Bishop.Commands.Weather;

public record WeatherBeaconEntity(float Min, float Max, WeatherMetric Type);