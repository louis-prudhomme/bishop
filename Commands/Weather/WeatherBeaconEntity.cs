using System;
using System.Collections.Generic;
using System.Linq;

namespace Bishop.Commands.Weather;

public record WeatherBeaconEntity(float Min, float Max, WeatherMetric Type, Dictionary<float, string>? Levels)
{
    private float Base => Max - Min;

    public float Ratio(float level)
    {
        return (level - Min) / Base;
    }

    public string LevelFor(float level)
    {
        if (Levels == null) return "";
        return Levels[Levels.Keys.OrderBy(item => Math.Abs(level - item)).First()];
    }
}