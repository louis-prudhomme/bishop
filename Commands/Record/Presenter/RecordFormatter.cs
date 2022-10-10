using System.Data;
using System.Threading.Tasks;
using Bishop.Commands.Record.Model;
using Bishop.Helper;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Presenter;

public class RecordFormatter
{
    public IKeyBasedCache<ulong, string> Cache { private get; set; } = null!;

    public async Task<string> FormatRecordRanking(ulong userId, CounterCategory category, long score, int? rank = null)
    {
        var username = await Cache.GetValue(userId) ?? throw new NoNullAllowedException();
        
        return FormatRecordRanking(username, category, score, rank);
    }

    public string FormatRecordRanking(DiscordMember member, CounterCategory category, long score, int? rank = null) =>
        FormatRecordRanking(member.Username, category, score, rank);

    private string FormatRecordRanking(string username, CounterCategory category, long score, int? rank = null)
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