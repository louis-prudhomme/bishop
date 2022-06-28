using System;
using System.Collections.Generic;
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

    private static readonly HoroscopeDb Full = new JsonDeserializer<HoroscopeDb>(HoroscopeFilePath)
        .Get()
        .Result;

    private readonly HoroscopeScraper _scraperService = new();

    public Random Rand { private get; set; } = null!;


    private static List<string> Links => Full.Links;
    private static List<HoroscopeSign> Signs => Full.Signs;

    [Command("horoscope")]
    [Aliases("Irma", "ho")]
    [Description("Prints a horoscope for @sign")]
    public async Task Predicting(CommandContext context,
        [Description("Horoscope sign")] [RemainingText]
        string userSign)
    {
        var found = false;

        foreach (var (sign, aliases) in Signs)
        foreach (var alias in aliases)
        {
            if (!alias.Equals(userSign, StringComparison.CurrentCultureIgnoreCase)) continue;

            found = true;
            var link = Links[Rand.Next(Links.Count)];
            var horoscope = _scraperService.GetHoroscopes(link, sign);
            var response = "*" + sign + "*\n" + horoscope;

            await context.RespondAsync(response);
        }

        if (!found) await context.RespondAsync("wtf is a " + userSign + "???");
    }

    private record HoroscopeDb(List<string> Links, List<HoroscopeSign> Signs);

    private record HoroscopeSign(string Sign, List<string> Aliases);
}