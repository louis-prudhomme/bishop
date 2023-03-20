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

    private string Singular(int number, string yes, string no) => number == 1 ? yes : no;

    public string FormatRecordRankingUpdate(DiscordMember member, CounterCategory category, long score, long previousScore, string reason) =>
        $"{member.Mention}'s {category.DisplayName()} score went from {previousScore} to **{score}** because {reason}";

    public string FormatScoreUpdate(string motive) =>
        $"«*{motive}*» was added to their history.";

    public string FormatGhostScoreUpdate(int count) =>
        $"**{count}** record{Singular(count, "", "s")} {Singular(count, "was", "were")} added to their history, for unknown reasons.";

    public string FormatBrokenMilestone(long milestone) => $"A new milestone has been broken through: {milestone}! 🎉";

    public string FormatRecordRanking(DiscordMember member, CounterCategory category, long score) =>
        FormatRecordRanking(member.Username, category, score, null);

    public string FormatRecordRanking(string username, CounterCategory category, long score, int? rank) =>
        $"{GetFormattedRank(rank).IfEmpty("\t  ")}{username}’s {category.DisplayName()} ⇒ {score}";

    private string GetFormattedRank(int? rank) => rank switch
    {
        0 => "🥇",
        1 => "🥈",
        2 => "🥉",
        _ => string.Empty
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
        var rank = GetFormattedRank(ranking);
        var formattedRank = rank.IsEmpty()
            ? $"**#{ranking + 1}**"
            : $"**#{ranking + 1}** {rank}";

        return $"*{member.Mention}* has accumulated **{score}** points and ranks {formattedRank} in **{category.DisplayName()}**"
               + "\n__Their last records are:__"
               + $"{TabulatedNewline}{records.Select(FormatRecord).JoinWith(TabulatedNewline)}";
    }

    public string FormatProgression(DiscordMember member, CounterCategory category, double ratio, int recordsSince, DateTime since) => ratio switch
    {
        0 => $"There's no progression for you in {category.DisplayName()} since {DateHelper.FromDateTimeToStringDate(since)}. How sad...",
        _ => $"{member.Mention} gained {recordsSince} in {category.DisplayName()} points since {DateHelper.FromDateTimeToStringDate(since)}"
    };
}