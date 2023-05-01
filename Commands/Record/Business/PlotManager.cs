using System;
using System.Collections.Generic;
using System.Linq;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using Microsoft.FSharp.Core;
using Plotly.NET;

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

    private List<(string Date, int Additions)> GetCountOfAdditionsByDay(IReadOnlyCollection<DateTime> allDates, (DateTime Min, DateTime Max)? timestamps = null)
    {
        var firstDate = timestamps?.Min ?? allDates.First();
        var lastDate = timestamps?.Max ?? allDates.Last();

        var datesString = allDates
            .Select(DateHelper.FromDateTimeToStringDate)
            .ToArray();
        return Enumerable
            .Range(0, lastDate.Subtract(firstDate).Days + 1)
            .Select(offset => firstDate.AddDays(offset))
            .Select(DateHelper.FromDateTimeToStringDate)
            .Select(date => (Date: date, Additions: datesString.Count(s => s.Equals(date))))
            .Where(tuple => timestamps != null || tuple.Additions != 0)
            .ToList();
    }

    private IEnumerable<string> GetListOfTagsForAbnormalBumps(IReadOnlyCollection<(string Date, int Additions)> allAdditions)
    {
        var allBumps = allAdditions
            .Select(tuple => tuple.Additions)
            .Where(additions => additions != 0)
            .OrderBy(i => i)
            .ToList();
        var firstQuartile = allBumps.Skip(allBumps.Count * 1 / 4).Take(1).First();
        var thirdQuartile = allBumps.Skip(allBumps.Count * 3 / 4).Take(1).First();
        var interQuartile = thirdQuartile - firstQuartile;
        var higherExternalBound = interQuartile + thirdQuartile * 2;
        higherExternalBound = higherExternalBound <= 2 ? int.MaxValue : higherExternalBound;

        return allAdditions
            .Select(tuple => tuple.Additions >= higherExternalBound
                ? tuple.Date
                : string.Empty);
    }

    public PlotImage Cumulative(List<RecordEntity> records)
    {
        var dates = OrganizeAllDatesOfAdditionsForUserAndCategory(records);
        var allAdditions = GetCountOfAdditionsByDay(dates);
        var tags = GetListOfTagsForAbnormalBumps(allAdditions);

        var chart = Chart2D.Chart.Line(
            allAdditions.Select(tuple => tuple.Date),
            allAdditions.Select(tuple => tuple.Additions).CumulativeSum(),
            true,
            ShowLegend: false,
            MultiText: FSharpOption<IEnumerable<string>>.Some(tags),
            TextPosition: StyleParam.TextPosition.TopCenter);

        return new PlotImage(chart);
    }


    public PlotImage CumulativeBy<TGroupedBy>(List<RecordEntity> records,
        Func<RecordEntity, TGroupedBy> discriminator,
        Func<TGroupedBy, string> getDisplayName,
        Func<TGroupedBy, Color> getDisplayColor)
    {
        var charts = new List<GenericChart.GenericChart>();
        var recordsByCategories = records.GroupBy(discriminator).ToList();
        var timestamps = records.Select(record => record.Timestamp).ToList();

        var minimumDate = DateHelper.FromTimestampToDateTime(timestamps.Min());
        var maximumDate = DateHelper.FromTimestampToDateTime(timestamps.Max());
        foreach (var recordsByCategory in recordsByCategories)
        {
            var dates = OrganizeAllDatesOfAdditionsForUserAndCategory(recordsByCategory.Select(record => record).ToList());
            var allAdditions = GetCountOfAdditionsByDay(dates, (Min: minimumDate, Max: maximumDate));
            var tags = GetListOfTagsForAbnormalBumps(allAdditions);

            var chart = Chart2D.Chart.Line(
                allAdditions.Select(tuple => tuple.Date),
                allAdditions.Select(tuple => tuple.Additions).CumulativeSum(),
                false,
                getDisplayName(recordsByCategory.Key),
                true,
                MultiText: FSharpOption<IEnumerable<string>>.Some(tags),
                TextPosition: StyleParam.TextPosition.TopCenter,
                LineColor: getDisplayColor(recordsByCategory.Key));

            charts.Add(chart);
        }

        var combinedChart = GenericChart.combine(charts);
        return new PlotImage(combinedChart);
    }

    public PlotImage Histogram(List<RecordEntity> records)
    {
        var dates = OrganizeAllDatesOfAdditionsForUserAndCategory(records);
        var allAdditions = GetCountOfAdditionsByDay(dates);
        var tags = GetListOfTagsForAbnormalBumps(allAdditions);

        var chart = Chart2D.Chart.Column(
            allAdditions.Select(tuple => tuple.Additions),
            FSharpOption<IEnumerable<string>>.Some(allAdditions.Select(tuple => tuple.Date)),
            ShowLegend: false,
            MultiText: FSharpOption<IEnumerable<string>>.Some(tags),
            Base: FSharpOption<int>.None,
            Width: FSharpOption<int>.None,
            TextPosition: StyleParam.TextPosition.Outside
        );

        return new PlotImage(chart);
    }
}

internal static class CounterCategoryColorExtension
{
    internal static Color DisplayColor(this CounterCategory self)
    {
        return self switch
        {
            CounterCategory.Bdm => Color.fromKeyword(ColorKeyword.Aqua),
            CounterCategory.Sauce => Color.fromKeyword(ColorKeyword.Fuchsia),
            CounterCategory.Sel => Color.fromKeyword(ColorKeyword.Navy),
            CounterCategory.Beauf => Color.fromKeyword(ColorKeyword.Chartreuse),
            CounterCategory.Rass => Color.fromKeyword(ColorKeyword.Tan),
            CounterCategory.Malfoy => Color.fromKeyword(ColorKeyword.Crimson),
            CounterCategory.Wind => Color.fromKeyword(ColorKeyword.Brown),
            CounterCategory.Raclette => Color.fromKeyword(ColorKeyword.Beige),
            _ => throw new ArgumentOutOfRangeException(nameof(self), self, null)
        };
    }
}
