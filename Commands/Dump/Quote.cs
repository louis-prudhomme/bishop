using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using static Bishop.Config.QuoteConfigurator;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send quotes to @users.
/// </summary>
public class Quote : BaseCommandModule
{
    public Random Rand { private get; set; } = null!;
    public static List<Politician> Quotes { get; set; } = null!;

    [Command("quote")]
    [Aliases("quotes", "q")]
    [Description("Prints a quote of @person")]
    public async Task Quoting(CommandContext context,
        [Description("Person to quote")][RemainingText] string person)
    {
        Boolean quoted = false;
        List<string> list_aliases = new List<string>{ "liste", "list" };

        if (list_aliases.Contains(person))
        {
            quoted = true;
            string response = "Here are the current available people to quote: \n";
            foreach (Politician politician in Quotes)
            {
                response += politician.names[0] + "\n";
            }

            await context.RespondAsync(response);
        }

        foreach (Politician politician in Quotes)
        {
            foreach (string name in politician.names)
            {
                if (name.Equals(person, StringComparison.CurrentCultureIgnoreCase))
                {
                    quoted = true;
                    await context.RespondAsync($"*“{politician.quotes[Rand.Next(politician.quotes.Count)]}”*\n                                       - {politician.names[0]}");
                }
            }
        }

        if (!quoted)
        {
            await context.RespondAsync("Nom pas reconnu, probablement");
        }
    }
}