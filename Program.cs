using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.Record.Presenter;
using Bishop.Commands.Record.Presenter.Aliases;
using Bishop.Commands.Weather.Presenter;
using Bishop.Config;
using Bishop.Helper;
using Bishop.Helper.Grive;
using DSharpPlus;
using log4net;
using log4net.Config;

namespace Bishop;

internal static class Program
{
    private static readonly ILog Log = LogManager
        .GetLogger(MethodBase.GetCurrentMethod()?
            .DeclaringType);

    private static DiscordClient _discord = null!;
    private static DiscordClientGenerator _generator = null!;

    [STAThread]
    private static void Main()
    {
        XmlConfigurator.Configure();

        _generator = new DiscordClientGenerator();
        _generator.RegisterBulk(CommandClasses);

        _discord = _generator.Client;
        AdaptUserIdTo.UserNameAsync = async id => (await _discord.GetUserAsync(id)).Username;

        Log.Info($"Sigil is {_generator.Sigil}");
        Log.Info("Awaiting commands");

        new GriveWalker().BuildGriveCache();

        MainAsync()
            .GetAwaiter()
            .GetResult();
    }

    private static async Task MainAsync()
    {
        await _discord.ConnectAsync();
        await Task.Delay(-1);
    }
    
    /// <summary>
    /// Array containing all the classes that must be registered as commands through
    /// the <see cref="DiscordClientGenerator"/>.BulkRegister method.
    /// </summary>
    private static readonly Type[] CommandClasses = new List<Type>
        {
            typeof(Randomizer),
            typeof(Stalk),
            typeof(Tomato),
            typeof(Aled),
            typeof(Horoscope),
            typeof(Quote),
            typeof(Vote),
            typeof(Piggies),
            typeof(Deleter),
            typeof(BdmCounterController),
            typeof(BeaufCounterController),
            typeof(SauceCounterController),
            typeof(MalfoyCounterController),
            typeof(SelCounterController),
            typeof(WindCounterController),
            typeof(RassCounterController),
            typeof(RecordController),
            typeof(CardGameService),
            typeof(WeatherController),
            typeof(UserNameCacheService),
        }.ToArray();
}
