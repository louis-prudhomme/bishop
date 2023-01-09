using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Model;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.FSharp.Core;
using Plotly.NET;
using Plotly.NET.ImageExport;
using PuppeteerSharp.Input;

namespace Bishop.Commands.Record.Presenter;

/// <summary>
///     The <c>Counter</c>-part of the <c>RecordController</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
/// </summary>
public partial class RecordController
{
    [Command("cumulative")]
    public async Task Cumulative(CommandContext context, DiscordMember member, CounterCategory category)
    {
        try
        {
            var dates = await FetchAllDatesOfAdditionsForUserAndCategory(member, category);
            var allAdditions = GetCountOfAdditionsByDay(dates);
            var tags = GetListOfTagsForAbnormalBumps(allAdditions);

            var graph = Chart2D.Chart.Line(
                allAdditions.Select(tuple => tuple.Count).CumulativeSum(),
                allAdditions.Select(tuple => tuple.Key),
                true,
                FSharpOption<string>.None,
                true,
                FSharpOption<double>.None,
                FSharpOption<IEnumerable<double>>.None,
                FSharpOption<string>.None,
                FSharpOption<IEnumerable<string>>.Some(tags),
                StyleParam.TextPosition.TopCenter);

            var builder = new DiscordMessageBuilder();
            var uuid = Guid.NewGuid();
            var filename = $"./{uuid}.jpg";
            graph.SaveJPG(uuid.ToString());
            builder.WithFile(File.Open(filename, FileMode.Open));
            await context.RespondAsync(builder);
            File.Delete(filename);
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }


    private async Task<List<DateTime>> FetchAllDatesOfAdditionsForUserAndCategory(DiscordMember member,
        CounterCategory category)
    {
        var records = await RecordRepository.FindByUserAndCategory(member.Id, category);
        return records
            .Select(record => record.Timestamp)
            .Select(DateHelper.FromTimestampToDateTime)
            .Where(time => time >= DateHelper.BishopEpoch) // discard legacy placeholder dates, which are placed in 1970
            .OrderBy(date => date)
            .ToList();
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

    private IEnumerable<string> GetListOfTagsForAbnormalBumps(List<Countach> allAdditions)
    {
        var allBumps = allAdditions
            .Select(tuple => tuple.Count)
            .OrderBy(i => i)
            .ToList();
        var firstQuartile = allBumps.Skip(allAdditions.Count * 1 / 4).Take(1).First();
        var thirdQuartile = allBumps.Skip(allAdditions.Count * 3 / 4).Take(1).First();
        var interquartile = thirdQuartile - firstQuartile;
        var higherExternalBound = interquartile + thirdQuartile * 3;
        higherExternalBound = higherExternalBound <= 2 ? int.MaxValue : higherExternalBound;

        return allAdditions
            .Select(tuple => tuple.Count >= higherExternalBound
                ? tuple.Key
                : string.Empty);
    }
}