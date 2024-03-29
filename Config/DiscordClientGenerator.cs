using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.Horoscope;
using Bishop.Commands.Record.Business;
using Bishop.Commands.Record.Controller;
using Bishop.Commands.Record.Domain;
using Bishop.Commands.Weather.Domain;
using Bishop.Commands.Weather.Service;
using Bishop.Helper;
using Bishop.Helper.Grive;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using log4net;
using Microsoft.Extensions.DependencyInjection;

namespace Bishop.Config;

/// <summary>
///     Generates the configuration of the Discord Client.
/// </summary>
public class DiscordClientGenerator
{
    private static readonly ILog Log = LogManager
        .GetLogger(MethodBase.GetCurrentMethod()?
            .DeclaringType);

    private static readonly string DiscordToken = Environment
        .GetEnvironmentVariable("DISCORD_TOKEN")!;

    private readonly SlashCommandsExtension _commands;

    private readonly VoteAnswerEventHandler _booth = new();

    public DiscordClientGenerator()
    {
        Client = new DiscordClient(AssembleConfig());

        Client.ComponentInteractionCreated += _booth.Handle;
        _commands = Client.UseSlashCommands(AssembleCommands(AssembleServiceCollection()));
        _commands.SlashCommandErrored += (_, args) =>
        {
            Log.Error($"[{DateTime.Now}][{args.Context}]: {args.Exception}");
            return Task.CompletedTask;
        };
    }

    public DiscordClient Client { get; }

    private IServiceCollection AssembleServiceCollection()
    {
        UserNameAccessor.FetchUserName = async id => (await Client.GetUserAsync(id)).Username;
        // TODO find a better way to add conditions per-folder rather than globally
        var griveWalker = new GriveWalker(new List<FileCheck>
        {
            Path.HasExtension,
            path => GriveWalker.AuthorizedExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()),
            path => new FileInfo(path).Length < GriveWalker.DiscordFileSizeLimitBytes
        });
        // CACHES
        var nestedGriveCache =
            new AutoUpdatingKeyBasedCache<GriveDirectory, ImmutableList<string>>(GriveWalker.CacheForSeconds,
                griveWalker.FetchFilesAsync);
        var nestedWeatherCache =
            new AutoUpdatingKeyBasedCache<string, WeatherEntity>(WeatherAccessor.CacheForSeconds,
                WeatherAccessor.Current);
        var nestedUserNameCacheService =
            new AutoUpdatingKeyBasedCache<ulong, string>(UserNameAccessor.CacheForSeconds,
                UserNameAccessor.FetchUserName);
        // OTHERS
        var nestedRecordRepository = new RecordRepository();
        var nestedRecordManager = new RecordManager
        {
            Repository = nestedRecordRepository
        };
        var nestedPlotManager = new PlotManager();
        var nestedRecordsController = new RecordController
        {
            Cache = nestedUserNameCacheService,
            Random = new Random(),
            Formatter = new RecordFormatter(),
            Manager = nestedRecordManager,
            PlotManager = nestedPlotManager
        };
        var nestedWeatherService = new WeatherService
        {
            Cache = nestedWeatherCache
        };
        var nestedCardGameFormatter = new CardGameFormatter
        {
            Cache = nestedUserNameCacheService
        };

        return new ServiceCollection()
            .AddSingleton(_booth)
            .AddSingleton(nestedRecordsController)
            .AddSingleton(nestedWeatherService)
            .AddSingleton<IKeyBasedCache<GriveDirectory, ImmutableList<string>>>(nestedGriveCache)
            .AddSingleton<IKeyBasedCache<string, WeatherEntity>>(nestedWeatherCache)
            .AddSingleton<IKeyBasedCache<ulong, string>>(nestedUserNameCacheService)
            .AddSingleton(nestedPlotManager)
            .AddSingleton(nestedRecordManager)
            .AddSingleton(nestedCardGameFormatter)
            .AddSingleton<RecordFormatter>()
            .AddSingleton<CardGameRepository>()
            .AddSingleton<HoroscopeRepository>();
    }

    private SlashCommandsConfiguration AssembleCommands(IServiceCollection services)
    {
        return new SlashCommandsConfiguration
        {
            Services = services.BuildServiceProvider()
        };
    }

    private static DiscordConfiguration AssembleConfig()
    {
        return new DiscordConfiguration
        {
            Token = DiscordToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers | DiscordIntents.MessageContents
        };
    }

    public void RegisterBulk(params Type[] types)
    {
        foreach (var type in types) _commands.RegisterCommands(type);
    }
}
