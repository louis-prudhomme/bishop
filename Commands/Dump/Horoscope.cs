using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

/// <summary>
///     Provide a command to send horoscopes to @users.
/// </summary>
public class Horoscope : BaseCommandModule
{
    public HoroscopeRepository Repository { private get; set; } = null!;

    private const string HoroscopeFilePath = "horoscopes.json";

    private static readonly HoroscopeDb Db = new JsonDeserializer<HoroscopeDb>(HoroscopeFilePath)
        .Get()
        .Result;

    private readonly HoroscopeScraper _scraperService = new();

    private readonly Random _rand = new();

    [Command("horoscope")]
    [Aliases("Irma", "ho")]
    [Description("Prints a horoscope for @sign")]
    public async Task Predicting(CommandContext context,
        [Description("Horoscope sign")] [RemainingText]
        string userSign)
    {
        var horoscopes = await Repository.FindAllAsync();

        var response = "";

        var horoscopeSign = Signs.FirstOrDefault(HoroscopeSign => HoroscopeSign.Aliases.Contains(userSign, StringComparer.InvariantCultureIgnoreCase));

        if (horoscopeSign == null)
        {
            response = "wtf is a " + userSign + "???";
        }
        else
        {
            response = "*" + horoscopeSign.Name + "*\n";

            var baseHoroscope = horoscopes.FirstOrDefault(horoscopesBase => horoscopeSign.Name.Equals(horoscopesBase.baseSign, StringComparison.CurrentCultureIgnoreCase));

            if (baseHoroscope == null)
            {
                string link = Links[_rand.Next(Links.Count)];
                string horoscope = _scraperService.GetHoroscopes(link, horoscopeSign.Name).Result;

                var newHoroscope = new HoroscopeEntity(horoscopeSign.Name, horoscope);
                response += newHoroscope.horoscope.ToString();

                await Repository.SaveAsync(newHoroscope);
            }
            else
            {
                if (DateHelper.FromTimestampToDateTime(baseHoroscope.timestamp).Date != DateTime.Now.Date)
                {
                    string link = Links[_rand.Next(Links.Count)];
                    string horoscope = _scraperService.GetHoroscopes(link, horoscopeSign.Name).Result;

                    baseHoroscope.ReplaceHoroscope(horoscope);
                    response += baseHoroscope.horoscope.ToString();

                    await Repository.SaveAsync(baseHoroscope);
                }
                else
                {
                    response += baseHoroscope.horoscope.ToString();
                }
            }
        }

        await context.RespondAsync(response);
    }


    private static List<string> Links => Db.Links;
    private static List<HoroscopeSign> Signs => Db.Signs;

    private record HoroscopeDb(List<string> Links, List<HoroscopeSign> Signs);

    private record HoroscopeSign(string Name, List<string> Aliases);
}