using System;
using System.Reflection;
using System.Threading.Tasks;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using Bishop.Commands.Meter.Aliases;
using Bishop.Commands.Weather;
using Bishop.Config;
using Bishop.Helper;
using DSharpPlus;
using log4net;
using log4net.Config;

namespace Bishop;

internal class Program
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

        _generator.Register<Randomizer>();
        _generator.Register<Stalk>();
        _generator.Register<Tomato>();
        _generator.Register<Vote>();
        _generator.Register<Deleter>();

        _generator.Register<BdmCounter>();
        _generator.Register<BeaufCounter>();
        _generator.Register<SauceCounter>();
        _generator.Register<SelCounter>();
        _generator.Register<RassCounter>();

        _generator.Register<CounterService>();
        _generator.Register<RecordService>();
        _generator.Register<CardGameService>();

        _generator.Register<WeatherController>();

        _generator.Register<UserNameCacheService>();

        _discord = _generator.Client;
        AdaptUserIdTo.UserNameAsync = async id => (await _discord.GetUserAsync(id)).Username;
        GuildFetcher.FetchAsync = async id => await _discord.GetGuildAsync(id);

        Log.Info($"Sigil is {_generator.Sigil}");
        Log.Info("Awaiting commands");

        MainAsync()
            .GetAwaiter()
            .GetResult();
    }

    private static async Task MainAsync()
    {
        await _discord.ConnectAsync();
        await Task.Delay(-1);
    }
}