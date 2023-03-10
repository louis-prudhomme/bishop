using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Business;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.FSharp.Core;
using Plotly.NET;
using Plotly.NET.ImageExport;
using Plotly.NET.TraceObjects;

namespace Bishop.Commands.Record.Controller;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordController</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
/// </summary>
public partial class RecordController
{
    [Command("cumulative")]
    public async Task Cumulative(CommandContext context, DiscordMember member, CounterCategory category)
    {
        var dates = await FetchAllDatesOfAdditionsForUserAndCategory(member, category);
        var allAdditions = GetCountOfAdditionsByDay(dates);
        var tags = GetListOfTagsForAbnormalBumps(allAdditions);

        var graph = Chart2D.Chart.Line(
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

        await SendGraph(context, graph);
    }

    [Command("histogram")]
    public async Task Histogram(CommandContext context, DiscordMember member, CounterCategory category)
    {
        var dates = await FetchAllDatesOfAdditionsForUserAndCategory(member, category);
        var allAdditions = GetCountOfAdditionsByDay(dates);
        var tags = GetListOfTagsForAbnormalBumps(allAdditions);

        var graph = Chart2D.Chart.Column(
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

        await SendGraph(context, graph);
    }


    private async Task<List<DateTime>> FetchAllDatesOfAdditionsForUserAndCategory(SnowflakeObject member,
        CounterCategory category)
    {
        var records = await Manager.Find(member.Id, category);
        return records
            .Select(record => record.Timestamp)
            .Select(DateHelper.FromTimestampToDateTime)
            .Where(time => time >= DateHelper.BishopEpoch) // discard legacy placeholder dates, which are placed in 1970
            .OrderBy(date => date)
            .ToList();
    }

    private async Task SendGraph(CommandContext context, GenericChart.GenericChart graph)
    {
        var builder = new DiscordMessageBuilder();
        using var file = new DisposableImage();
        var temp = await context.RespondAsync("Sending...");
        graph.SaveJPG(file.FilenameWithoutExtension);
        builder.WithFile(file.Stream());
        await context.RespondAsync(builder);
        await temp.DeleteAsync();
    }

    record Countach(string Key, int Count);

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
        var interquartile = thirdQuartile - firstQuartile;
        var higherExternalBound = interquartile + thirdQuartile * 2;
        higherExternalBound = higherExternalBound <= 2 ? int.MaxValue : higherExternalBound;

        return allAdditions
            .Select(tuple => tuple.Count >= higherExternalBound
                ? tuple.Key
                : string.Empty);
    }
}