using System;
using System.Collections.Generic;
using System.Linq;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using DSharpPlus.Entities;
using static System.Int64;

namespace Bishop.Commands.Record.Controller;

public class RecordFormatter
{
    public const string TabulatedNewline = "\n\t";
    public const string RankTabulation = "\t  ";

    private string Singular(int number, string yes, string no)
    {
        return number == 1 ? yes : no;
    }

    public string FormatRecordRankingUpdate(DiscordUser user, CounterCategory category, long score, long previousScore, string reason)
    {
        return $"{user.Mention}'s {category.DisplayName()} score went from {previousScore} to **{score}** because {reason}";
    }

    public string FormatScoreUpdate(string motive)
    {
        return $"«*{motive}*» was added to their history.";
    }

    public string FormatGhostScoreUpdate(int count)
    {
        return $"**{count}** record{Singular(count, "", "s")} {Singular(count, "was", "were")} added to their history, for unknown reasons.";
    }

    public string FormatBrokenMilestone(long milestone)
    {
        return $"A new milestone has been broken through: {milestone}! 🎉";
    }

    public string FormatSimpleRecordRanking(int? rank, CounterCategory category, long score)
    {
        var formattedRank = GetFormattedRank(rank);
        formattedRank = formattedRank == string.Empty ? RankTabulation : $"{formattedRank}";
        var numericRank = rank == null ? "Not ranked" : $"**#{rank + 1}**";
        return $"{formattedRank} {numericRank} in **{category.DisplayName()}** with **{score}** points";
    }

    public string FormatRecordRanking(DiscordUser user, CounterCategory category, long score)
    {
        return FormatRecordRanking(user.Username, category, score, null);
    }

    public string FormatRecordRanking(string username, CounterCategory category, long score, int? rank)
    {
        return $"{GetFormattedRank(rank).IfEmpty(RankTabulation)}{username}’s {category.DisplayName()} ⇒ {score}";
    }

    private string GetFormattedRank(int? rank)
    {
        return rank switch
        {
            0 => "🥇",
            1 => "🥈",
            2 => "🥉",
            _ => string.Empty
        };
    }

    public string FormatRecordWithCategory(RecordEntity toFormat)
    {
        return FormatRecord(toFormat, true);
    }

    public string FormatRecord(RecordEntity toFormat)
    {
        return FormatRecord(toFormat, false);
    }

    private string FormatRecord(RecordEntity toFormat, bool shouldIncludeCategory)
    {
        var reason = toFormat.Motive == null
            ? "*For reasons unknown to History*"
            : $"*« {toFormat.Motive} »*";

        if (toFormat.Category == CounterCategory.Raclette)
            reason = TryParse(toFormat.Motive, out var motiveLong)
                ? "*Unknown Raclette*"
                : $"*« Raclette of {DateHelper.FromTimestampToStringDate(motiveLong)} »*";

        return shouldIncludeCategory
            ? $"{reason} – {DateHelper.FromDateTimeToStringDate(toFormat.RecordedAt)} in **{toFormat.Category.DisplayName()}**"
            : $"{reason} – {DateHelper.FromDateTimeToStringDate(toFormat.RecordedAt)}";
    }

    public string FormatLongRecord(DiscordUser user, CounterCategory category, int? ranking, long score, IEnumerable<RecordEntity> records)
    {
        var rank = GetFormattedRank(ranking);
        var numericRank = ranking == null ? "Not ranked" : $"**#{rank + 1}**";
        var formattedRank = rank.IsEmpty()
            ? $"{numericRank}"
            : $"{numericRank} {rank}";

        return $"*{user.Mention}* has accumulated **{score}** points and ranks {formattedRank} in **{category.DisplayName()}**"
               + "\n__Their last records are:__"
               + $"{TabulatedNewline}{records.Select(FormatRecord).JoinWith(TabulatedNewline)}";
    }

    public string FormatProgression(DiscordUser user, CounterCategory category, double ratio, int recordsSince, DateTime since)
    {
        return ratio switch
        {
            0 => $"There's no progression for you in {category.DisplayName()} since {DateHelper.FromDateTimeToStringDate(since)}. How sad...",
            _ => $"{user.Mention} gained {recordsSince} in {category.DisplayName()} points since {DateHelper.FromDateTimeToStringDate(since)}"
        };
    }

    public string FormatRecap(DiscordUser user, IEnumerable<string> lines)
    {
        return $"__{user.Mention}'s full recap:__\n\t" + lines.JoinWith(TabulatedNewline);
    }
}
