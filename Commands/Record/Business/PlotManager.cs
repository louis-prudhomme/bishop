using System;
using System.Collections.Generic;
using System.Linq;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using Microsoft.FSharp.Core;
using Plotly.NET;
using Plotly.NET.TraceObjects;

namespace Bishop.Commands.Record.Business;

public class PlotManager
{
    private List<DateTime> OrganizeAllDatesOfAdditionsForUserAndCategory(List<RecordEntity> records)
    {
        return records
            .Select(record => record.Timestamp)
            .Select(DateHelper.FromTimestampToDateTime)
            .Where(time => time >= DateHelper.BishopEpoch) // discard legacy placeholder dates, which are placed in 1970
            .OrderBy(date => date)
            .ToList();
    }

    private List<Countach> GetCountOfAdditionsByDay(IReadOnlyCollection<DateTime> allDates)
    {
        var firstDate = allDates.First();
        var lastDate = allDates.Last();

        var datesString = allDates
            .Select(DateHelper.FromDateTimeToStringDate)
            .ToArray();
        return Enumerable
            .Range(0, lastDate.Subtract(firstDate).Days + 1)
            .Select(offset => firstDate.AddDays(offset))
            .Select(DateHelper.FromDateTimeToStringDate)
            .Select(date => new Countach(date, datesString.Count(s => s.Equals(date))))
            .Where(tuple => tuple.Count != 0)
            .ToList();
    }

    private IEnumerable<string> GetListOfTagsForAbnormalBumps(IReadOnlyCollection<Countach> allAdditions)
    {
        var allBumps = allAdditions
            .Select(tuple => tuple.Count)
            .OrderBy(i => i)
            .ToList();
        var firstQuartile = allBumps.Skip(allAdditions.Count * 1 / 4).Take(1).First();
        var thirdQuartile = allBumps.Skip(allAdditions.Count * 3 / 4).Take(1).First();
        var interQuartile = thirdQuartile - firstQuartile;
        var higherExternalBound = interQuartile + thirdQuartile * 2;
        higherExternalBound = higherExternalBound <= 2 ? int.MaxValue : higherExternalBound;

        return allAdditions
            .Select(tuple => tuple.Count >= higherExternalBound
                ? tuple.Key
                : string.Empty);
    }

    public PlotImage Cumulative(List<RecordEntity> records)
    {
        var dates = OrganizeAllDatesOfAdditionsForUserAndCategory(records);
        var allAdditions = GetCountOfAdditionsByDay(dates);
        var tags = GetListOfTagsForAbnormalBumps(allAdditions);

        var chart = Chart2D.Chart.Line(
            allAdditions.Select(tuple => tuple.Key),
            allAdditions.Select(tuple => tuple.Count).CumulativeSum(),
            true,
            FSharpOption<string>.None,
            false,
            FSharpOption<double>.None,
            FSharpOption<IEnumerable<double>>.None,
            FSharpOption<string>.None,
            FSharpOption<IEnumerable<string>>.Some(tags),
            StyleParam.TextPosition.TopCenter);

        return new PlotImage(chart);
    }

    public PlotImage Histogram(List<RecordEntity> records)
    {
        var dates = OrganizeAllDatesOfAdditionsForUserAndCategory(records);
        var allAdditions = GetCountOfAdditionsByDay(dates);
        var tags = GetListOfTagsForAbnormalBumps(allAdditions);

        var chart = Chart2D.Chart.Column(
            allAdditions.Select(tuple => tuple.Count),
            FSharpOption<IEnumerable<string>>.Some(allAdditions.Select(tuple => tuple.Key)),
            null,
            false,
            null,
            null,
            null,
            FSharpOption<IEnumerable<string>>.Some(tags),
            FSharpOption<Color>.None,
            FSharpOption<StyleParam.Colorscale>.None,
            FSharpOption<Line>.None,
            FSharpOption<StyleParam.PatternShape>.None,
            FSharpOption<IEnumerable<StyleParam.PatternShape>>.None,
            FSharpOption<Pattern>.None,
            FSharpOption<Marker>.None,
            FSharpOption<int>.None,
            FSharpOption<int>.None,
            FSharpOption<IEnumerable<int>>.None,
            FSharpOption<StyleParam.TextPosition>.Some(StyleParam.TextPosition.Outside)
        );

        return new PlotImage(chart);
    }


    private record Countach(string Key, int Count);
}