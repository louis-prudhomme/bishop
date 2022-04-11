using System;
using System.Collections.Generic;
using System.Linq;

namespace Bishop.Commands.Weather;

public record WeatherDescription(float Level, string Description);
public record WeatherBeaconEntity(float Min, float Max, WeatherMetric Type, List<WeatherDescription>? Levels)
{
    private const float Tolerance = 0.00001f;
    private static readonly Random Random = new();
    
    private float Base => Max - Min;

    public float Ratio(float level)
    {
        return (level - Min) / Base;
    }

    public string LevelFor(float targetLevel)
    {
        if (Levels == null) return "";

        var closestLevel = Levels
            .DistinctBy(description => description.Level)
            .Select(description => description.Level)
            .OrderBy(theoreticalLevel => Math.Abs(targetLevel - theoreticalLevel))
            .First();
        
        var suitableLines = Levels
            .Where(description => Math.Abs(description.Level - closestLevel) < Tolerance)
            .Select(description => description.Description)
            .ToList();

        return suitableLines.ElementAt(Random.Next(suitableLines.Count));
    }
}

