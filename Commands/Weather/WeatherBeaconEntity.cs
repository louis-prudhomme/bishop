using System.Collections.Generic;

namespace Bishop.Commands.Weather;

public record WeatherBeaconEntity(float Min, float Max, WeatherMetric Type)
{
    private float Base => Max - Min;

    public float Ratio(float level)
    {
        return (level - Min) / Base;
    }
}