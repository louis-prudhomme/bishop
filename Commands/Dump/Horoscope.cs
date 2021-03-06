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
        var sign = Signs.FirstOrDefault(horoscopeSign => horoscopeSign
            .Aliases.Contains(userSign, StringComparer.InvariantCultureIgnoreCase)); 
        
        if (sign == null)
        {
            await context.RespondAsync("wtf is a " + userSign + "???");
            return;
        }

        var link = Links[_rand.Next(Links.Count)];
        // TODO: add caching
        var horoscope = await _scraperService.GetHoroscopes(link, sign.Name);
        var response = "*" + sign.Name + "*\n" + horoscope;
        await context.RespondAsync(response);
    }


    private static List<string> Links => Db.Links;
    private static List<HoroscopeSign> Signs => Db.Signs;

    private record HoroscopeDb(List<string> Links, List<HoroscopeSign> Signs);

    private record HoroscopeSign(string Name, List<string> Aliases);
}