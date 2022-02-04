using System;
using System.Reflection;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using Bishop.Config.Converters;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using log4net;
using Microsoft.Extensions.DependencyInjection;

namespace Bishop.Config;

/// <summary>
///     Generates the configuration of the Discord Client. Coucou
/// </summary>
public class DiscordClientGenerator
{
    /// <summary>
    ///     Can be overriden by environment variables. See <see cref="Program" />.
    /// </summary>
    private static readonly string[] Prefix = {";"};

    private readonly CommandsNextExtension _commands;

    private readonly ILog _logger = LogManager
        .GetLogger(MethodBase.GetCurrentMethod()?
            .DeclaringType);

    private readonly string?[] _sigil;

    private readonly string _token;

    public DiscordClientGenerator(string token, string? sigil, MongoContext dbContext)
    {
        _token = token;
        _sigil = new[] {sigil};
        Client = new DiscordClient(AssembleConfig());

        _commands = Client.UseCommandsNext(AssembleCommands(AssembleServiceCollection(dbContext)));
        _commands.SetHelpFormatter<DefaultHelpFormatter>();

        _commands.RegisterConverter(new MeterKeysConverter());
    }


    public DiscordClient Client { get; }
    public string Sigil => string.Join(" ", _sigil);

    private IServiceCollection AssembleServiceCollection(MongoContext dbContext)
    {
        var nestedCounterService = new CounterService
        {
            CounterRepository = new CounterRepository()
        };

        return new ServiceCollection()
            .AddSingleton<Random>()
            .AddSingleton(nestedCounterService)
            .AddSingleton(dbContext)
            .AddSingleton<UserNameCache>()
            .AddSingleton<RecordRepository>()
            .AddSingleton<CounterRepository>()
            .AddSingleton<CardGameRepository>();
    }

    private CommandsNextConfiguration AssembleCommands(IServiceCollection services)
    {
        return new CommandsNextConfiguration
        {
            Services = services.BuildServiceProvider(),
            StringPrefixes = _sigil ?? Prefix
        };
    }

    private DiscordConfiguration AssembleConfig()
    {
        return new DiscordConfiguration
        {
            Token = _token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
        };
    }

    public void Register<T>() where T : BaseCommandModule
    {
        _commands.RegisterCommands<T>();
    }
}