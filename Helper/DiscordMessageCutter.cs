using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Bishop.Helper;

//FIXME: i do not work as of right now
public delegate Task FollowUp(string paragraph);

public static class DiscordMessageCutter
{
    private const int MessageMaxNumberOfLines = 12;

    private static List<string> DispatchIntoParagraphs(IReadOnlyCollection<string> lines)
    {
        var paragraphs = new List<string>();

        for (var i = 0; i <= lines.Count; i += MessageMaxNumberOfLines)
            paragraphs.Add(lines
                .Skip(i)
                .Take(MessageMaxNumberOfLines)
                .Aggregate((acc, h) => string.Join("\n", acc, h)));

        return paragraphs;
    }

    public static async Task PaginateAnswer(List<string> toSend, FollowUp initialNext, string? firstLine = null)
    {
        if (firstLine != null)
            toSend.Insert(0, firstLine);

        var paragraphs = DispatchIntoParagraphs(toSend);

        var next = initialNext;
        //foreach (var paragraph in paragraphs) next = (await next(paragraph)).CreateResponseAsync;
    }
}