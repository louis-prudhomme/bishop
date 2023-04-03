using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;


using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Horoscope;

/// <summary>
///     Provide a command to send horoscopes to @users.
/// </summary>
public class HoroscopeController : ApplicationCommandModule
{
    private const string HoroscopeFilePath = "horoscopes.json";

    private static readonly HoroscopeDb Db = new JsonDeserializer<HoroscopeDb>(HoroscopeFilePath)
        .Get()
        .Result;

    private readonly Random _rand = new();

    private readonly HoroscopeScraper _scraperService = new();
    public HoroscopeRepository Repository { private get; set; } = null!;


    private static List<string> Links => Db.Links;
    private static List<HoroscopeSign> Signs => Db.Signs;

    [SlashCommand("horoscope", "Prints a horoscope for @sign")]
    public async Task Predicting(InteractionContext context,
        [OptionAttribute("Sign", "plcaeholder")]
        string userSign)
    {
        var horoscopes = await Repository.FindAllAsync();

        string response;
        var horoscopeSign = Signs.FirstOrDefault(sign =>
            sign.Aliases.Contains(userSign, StringComparer.InvariantCultureIgnoreCase));
        if (horoscopeSign == null)
        {
            response = "wtf is a " + userSign + "???";
        }
        else
        {
            response = "*" + horoscopeSign.Name + "*\n";

            var baseHoroscope = horoscopes.FirstOrDefault(horoscopesBase =>
                horoscopeSign.Name.Equals(horoscopesBase.BaseSign, StringComparison.CurrentCultureIgnoreCase));

            if (baseHoroscope == null)
            {
                var link = Links[_rand.Next(Links.Count)];
                var horoscope = _scraperService.GetHoroscopes(link, horoscopeSign.Name).Result;

                var newHoroscope = new HoroscopeEntity(horoscopeSign.Name, horoscope);
                response += newHoroscope.Horoscope;

                await Repository.SaveAsync(newHoroscope);
            }
            else
            {
                if (DateHelper.FromTimestampToDateTime(baseHoroscope.Timestamp).Date != DateTime.Now.Date)
                {
                    var link = Links[_rand.Next(Links.Count)];
                    var horoscope = _scraperService.GetHoroscopes(link, horoscopeSign.Name).Result;

                    baseHoroscope.ReplaceHoroscope(horoscope);
                    response += baseHoroscope.Horoscope;

                    await Repository.SaveAsync(baseHoroscope);
                }
                else
                {
                    response += baseHoroscope.Horoscope;
                }
            }
        }

        await context.CreateResponseAsync(response);
    }

    private record HoroscopeDb(List<string> Links, List<HoroscopeSign> Signs);

    private record HoroscopeSign(string Name, List<string> Aliases);
}