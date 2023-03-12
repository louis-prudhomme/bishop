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
    public string FormatRecordRankingUpdate(DiscordMember member, CounterCategory category, long score, long previousScore) =>
        $"{FormatRecordRanking(member.Username, category, score, null)} (from {previousScore})";

    public string FormatScoreUpdate(DiscordMember member, CounterCategory category, string motive) =>
        $"Added «*{motive}*» to {member.Mention}’s {category.DisplayName()} history.";

    public string FormatBrokenMilestone(long milestone) => $"A new milestone has been broken through: {milestone}! 🎉";

    public string FormatRecordRanking(DiscordMember member, CounterCategory category, long score) =>
        FormatRecordRanking(member.Username, category, score, null);

    public string FormatRecordRanking(string username, CounterCategory category, long score, int? rank) =>
        $"{FormatRank(rank) ?? "⠀ ⠀"}{username}’s {category.DisplayName()} ⇒ {score}";

    private string? FormatRank(int? rank) => rank switch
    {
        0 => "🥇",
        1 => "🥈",
        2 => "🥉",
        _ => null
    };

    public string FormatRecordWithCategory(RecordEntity toFormat) => FormatRecord(toFormat, true);
    public string FormatRecord(RecordEntity toFormat) => FormatRecord(toFormat, false);

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

    public string FormatLongRecord(DiscordMember member, CounterCategory category, int ranking, long score, IEnumerable<RecordEntity> records)
    {
        const string lineSeparator = "\n\t";
        var rank = FormatRank(ranking);
        
        return $"{member.Mention} has accumulated **{score}** points and ranks **#{ranking + 1}** {rank} in **{category.DisplayName()}**"
               + "\n__Their last records are:__"
               + $"{lineSeparator}{records.Select(FormatRecord).JoinWith(lineSeparator)}";
    }

    public string FormatProgression(DiscordMember member, CounterCategory category, double ratio, int recordsSince, DateTime since) => ratio switch
    {
        0 => $"There's no progression for you in {category.DisplayName()} since {DateHelper.FromDateTimeToStringDate(since)}. How sad...",
        _ => $"{member.Mention} gained {recordsSince} in {category.DisplayName()} points since {DateHelper.FromDateTimeToStringDate(since)}"
    };
}