using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Bishop.Helper;

public delegate Task<DiscordMessage> FollowUp(string paragraph);

public class DiscordMessageCutter
{
    private const int MessageMaxNumberOfLines = 12;

    public static List<string> DispatchIntoParagraphs(List<string> lines)
    {
        var paragraphs = new List<string>();

        for (var i = 0; i <= lines.Count; i += MessageMaxNumberOfLines)
        {
            paragraphs.Add(lines
                .Skip(i)
                .Take(MessageMaxNumberOfLines)
                .Aggregate((acc, h) => string.Join("\n", acc, h)));
        }

        return paragraphs;
    }

    public static async Task PaginateAnswer(List<string> toSend, FollowUp initialNext)
    {
        var paragraphs = DispatchIntoParagraphs(toSend);

        var next = initialNext;
        foreach (var paragraph in paragraphs)
        {
            next = (await next(paragraph)).RespondAsync;
        }
    }
}