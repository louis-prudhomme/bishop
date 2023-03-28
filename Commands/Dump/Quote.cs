using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using Bishop.Helper.Extensions;


using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send quotes to @users.
/// </summary>
public class Quote : ApplicationCommandModule
{
    private const string QuoteFilePath = "quotes.json";

    private static readonly List<Politician> Politicians = new JsonDeserializer<List<Politician>>(QuoteFilePath)
        .Get()
        .Result;

    private readonly Random _rand = new();

    [SlashCommand("quote", "Prints a quote of @person")]
    public async Task Quoting(InteractionContext context,
        [OptionAttribute("Person", "Who to quote ?")]
        string person)
    {
        var target = Politicians.FirstOrDefault(politician => politician.Names
            .Contains(person, StringComparer.InvariantCultureIgnoreCase));

        if (target == null) await context.CreateResponseAsync("Nom pas reconnu, probablement");
        await context.CreateResponseAsync($"*“{target?.Quotes.Random()}”*" +
                                   $"\n                                       - {target?.Names.First()}");
    }

    [SlashCommand("list", "List available people to quote")]
    public async Task ListQuotes(InteractionContext context)
    {
        var response = Politicians.Aggregate("Here are the current available people to quote:",
            (current, politician) => string.Join("\n - ", current, politician.Names.First()));

        await context.CreateResponseAsync(response);
    }

    private record Politician(List<string> Names, List<string> Quotes);
}