using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send quotes to @users.
/// </summary>
[Group("quote")]
[Aliases("quotes", "q")]
[Description("Prints a quote of @person")]
public class Quote : BaseCommandModule
{
    private const string QuoteFilePath = "quotes.json";

    private static readonly List<Politician> Politicians = new JsonDeserializer<List<Politician>>(QuoteFilePath)
        .Get()
        .Result;

    private readonly Random _rand = new();

    [GroupCommand]
    public async Task Quoting(CommandContext context,
        [Description("Person to quote")] [RemainingText]
        string person)
    {
        var target = Politicians.FirstOrDefault(politician => politician.Names
            .Contains(person, StringComparer.InvariantCultureIgnoreCase));

        if (target == null) await context.RespondAsync("Nom pas reconnu, probablement");
        await context.RespondAsync($"*“{target?.Quotes[_rand.Next(target.Quotes.Count)]}”*" +
                                   $"\n                                       - {target?.Names.First()}");
    }

    [Command("list")]
    [Aliases("liste")]
    public async Task ListQuotes(CommandContext context)
    {
        var response = Politicians.Aggregate("Here are the current available people to quote:",
            (current, politician) => string.Join("\n - ", current, politician.Names.First()));

        await context.RespondAsync(response);
    }

    private record Politician(List<string> Names, List<string> Quotes);
}