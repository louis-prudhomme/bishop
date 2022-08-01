using Bishop.Commands.Dump;
using Bishop.Helper;
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
    public HoroscopeRepository Repository { private get; set; } = null!;
    private readonly Random _rand = new();
    public static List<string> links { get; set; } = null!;
    public static List<HoroscopeSign> signs { get; set; } = null!;
    private readonly HoroscopeScraper _scraperService = new();

    [Command("horoscope")]
    [Aliases("Irma", "ho")]
    [Description("Prints a horoscope for @sign")]
    public async Task Predicting(CommandContext context,
        [Description("Horoscope sign")][RemainingText] string userSign)
    {
        var horoscopes = await Repository.FindAllAsync();

        var response = "";

        var horoscopeSign = signs.FirstOrDefault(HoroscopeSign => HoroscopeSign.aliases.Contains(userSign, StringComparer.InvariantCultureIgnoreCase));

        if (horoscopeSign == null)
        {
            response ="wtf is a " + userSign + "???";
        }
        else
        {
            var baseHoroscope = horoscopes.FirstOrDefault(horoscopesBase => horoscopeSign.sign.Equals(horoscopesBase.baseSign, StringComparison.CurrentCultureIgnoreCase));

            if (baseHoroscope == null)
            {
                string link = links[_rand.Next(links.Count)];
                string horoscope = _scraperService.GetHoroscopes(link, horoscopeSign.sign);

                var newHoroscope = new HoroscopeEntity(horoscopeSign.sign, horoscope);
                response = newHoroscope.horoscope.ToString();

                await Repository.SaveAsync(newHoroscope);
            }
            else
            {
                if (DateHelper.FromTimestampToDateTime(baseHoroscope.timestamp).Date != DateTime.Now.Date)
                {
                    string link = links[_rand.Next(links.Count)];
                    string horoscope = _scraperService.GetHoroscopes(link, horoscopeSign.sign);

                    baseHoroscope.ReplaceHoroscope(horoscope);
                    response = baseHoroscope.horoscope.ToString();

                    await Repository.SaveAsync(baseHoroscope);
                }
                else
                {
                    response = baseHoroscope.horoscope.ToString();
                }
            }
        }

        await context.RespondAsync(response);
    }
}
