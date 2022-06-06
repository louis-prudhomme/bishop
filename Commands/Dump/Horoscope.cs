using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HoroscopeConfigurator;

/// <summary>
///     Provide a command to send horoscopes to @users.
/// </summary>
public class Horoscope : BaseCommandModule
{
    public Random Rand { private get; set; } = null!;
    public static List<string> links { get; set; } = null!;
    public static List<HoroscopeSign> signs { get; set; } = null!;
    public HoroscopeScraper ScraperService = new HoroscopeScraper();

    [Command("horoscope")]
    [Aliases("Irma", "ho")]
    [Description("Prints a horoscope for @sign")]
    public async Task Predicting(CommandContext context,
        [Description("Horoscope sign")][RemainingText] string userSign)
    {
        Boolean found = false;

        foreach (HoroscopeSign sign in signs)
        {
            foreach (string alias in sign.aliases)
            {
                if (alias.Equals(userSign, StringComparison.CurrentCultureIgnoreCase))
                {
                    found = true;
                    string link = links[Rand.Next(links.Count)];
                    string horoscope = ScraperService.GetHoroscopes(link, sign.sign);
                    string response = "*" + sign.sign + "*\n" + horoscope;

                    await context.RespondAsync(response);
                }
            }
        }

        if (!found)
        {
            await context.RespondAsync("wtf is a " + userSign + "???");
        }
    }
}
