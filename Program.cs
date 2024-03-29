using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.Horoscope;
using Bishop.Commands.Record.Controller;
using Bishop.Commands.Record.Controller.Aliases;
using Bishop.Commands.Weather.Controller;
using Bishop.Config;
using DSharpPlus;
using DSharpPlus.Entities;
using log4net;
using log4net.Config;
using Plotly.NET.ImageExport;

namespace Bishop;

internal static class Program
{
    private static readonly ILog Log = LogManager
        .GetLogger(MethodBase.GetCurrentMethod()?
            .DeclaringType);

    private static readonly string? ChromiumPath = Environment
        .GetEnvironmentVariable("CHROMIUM_PATH");

    private static readonly string? BotStatus = Environment
        .GetEnvironmentVariable("BOT_STATUS");

    private static DiscordClient _discord = null!;
    private static DiscordClientGenerator _generator = null!;

    /// <summary>
    ///     Array containing all the classes that must be registered as commands through
    ///     the <see cref="DiscordClientGenerator" />.BulkRegister method.
    /// </summary>
    private static readonly Type[] CommandClasses = new List<Type>
    {
        typeof(Randomizer),
        typeof(Stalk),
        typeof(Aled),
        typeof(Tomato),
        typeof(Piggies),
        typeof(Referendum),
        typeof(HoroscopeController),
        typeof(Quote),
        typeof(Deleter),
        typeof(CardGameService),
        typeof(WeatherController),
        typeof(RacletteCounterController),
        typeof(BdmCounterController),
        typeof(BeaufCounterController),
        typeof(RassCounterController),
        typeof(SauceCounterController),
        typeof(MalfoyCounterController),
        typeof(SelCounterController),
        typeof(WindCounterController),
        typeof(RecordController),
    }.ToArray();

    [STAThread]
    private static void Main()
    {
        XmlConfigurator.Configure();

        if (ChromiumPath != null)
        {
            PuppeteerSharpRendererOptions.localBrowserExecutablePath = ChromiumPath;
            PuppeteerSharpRendererOptions.launchOptions.Args = new[] {"--no-sandbox"};
        }

        _generator = new DiscordClientGenerator();
        _generator.RegisterBulk(CommandClasses);

        _discord = _generator.Client;

        Log.Info("Awaiting commands");

        MainAsync()
            .GetAwaiter()
            .GetResult();
    }

    private static async Task MainAsync()
    {
        await _discord.ConnectAsync(new DiscordActivity(BotStatus, ActivityType.Competing));
        await Task.Delay(-1);
    }
}
