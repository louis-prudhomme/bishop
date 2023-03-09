using System.Data;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Controller;

public class RecordFormatter
{
    public string FormatRecordRankingUpdate(DiscordMember member, CounterCategory category, long score,
        long previousScore) =>
        $"{FormatRecordRanking(member.Username, category, score, null)} (from {previousScore})";

    public string FormatBrokenMilestone(long milestone) => $"A new milestone has been broken through: {milestone}! 🎉";

    public string FormatRecordRanking(DiscordMember member, CounterCategory category, long score) =>
        FormatRecordRanking(member.Username, category, score, null);

    public string FormatRecordRanking(string username, CounterCategory category, long score, int? rank)
    {
        var displayedRank = rank switch
        {
            0 => "🥇 ",
            1 => "🥈 ",
            2 => "🥉 ",
            null => string.Empty,
            _ => "⠀ ⠀"
        };

        return $"{displayedRank}{username}’s {category} ⇒ {score}";
    }

    public string FormatRecordWithCategory(RecordEntity toFormat) => FormatRecord(toFormat, true);
    public string FormatRecord(RecordEntity toFormat) => FormatRecord(toFormat, false);

    private string FormatRecord(RecordEntity toFormat, bool shouldIncludeCategory)
    {
        var reason = "";
        if (toFormat.Category.Equals(CounterCategory.Raclette))
        {
            long MotiveLong;
            long.TryParse(toFormat.Motive, out MotiveLong);
            reason = toFormat.Motive == null
                ? "*Unknown Raclette*"
                : $"*« Raclette of {DateHelper.FromTimestampToDateTime(MotiveLong).ToString("dd-MM-yyyy")} »*";
        }
        else
        {
            reason = toFormat.Motive == null
                ? "*For reasons unknown to History*"
                : $"*« {toFormat.Motive} »*";
        }

        return shouldIncludeCategory
            ? $"{reason} – {DateHelper.FromDateTimeToStringDate(toFormat.RecordedAt)} in **{toFormat.Category}**"
            : $"{reason} – {DateHelper.FromDateTimeToStringDate(toFormat.RecordedAt)}";
    }
}