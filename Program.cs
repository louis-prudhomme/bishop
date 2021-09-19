using System;
using System.Reflection;
using System.Threading.Tasks;
using Bishop.Commands;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using Bishop.Config;
using DSharpPlus;
using log4net;
using log4net.Config;
using MongoDB.Driver;

namespace Bishop
{
    internal class Program
    {
        private const string TOMATO_FILE_PATH = "./Resources/tomatoes.json";
        private const string STALK_FILE_PATH = "./Resources/slenders.json";

        private static readonly string _DISCORD_TOKEN = Environment
            .GetEnvironmentVariable("DISCORD_TOKEN");

        private static readonly string _MONGO_TOKEN = Environment
            .GetEnvironmentVariable("MONGO_TOKEN");

        private static readonly string _MONGO_DATABASE = Environment
            .GetEnvironmentVariable("MONGO_DB");

        private static readonly string _COMMAND_SIGIL = Environment
            .GetEnvironmentVariable("COMMAND_SIGIL");

        private static readonly ILog _LOG = LogManager
            .GetLogger(MethodBase.GetCurrentMethod()?
                .DeclaringType);

        private static DiscordClient _discord;
        private static DiscordClientGenerator _generator;

        [STAThread]
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Tomato.Tomatoes = new TomatoConfigurator(TOMATO_FILE_PATH)
                .ReadTomatoesAsync()
                .Result;

            Stalk.Lines = new StalkConfigurator(STALK_FILE_PATH)
                .ReadStalkAsync()
                .Result;

            Enumerat.Database = _MONGO_DATABASE;
            Enumerat.Mongo = new MongoClient(_MONGO_TOKEN);
            _generator = new DiscordClientGenerator(_DISCORD_TOKEN, _COMMAND_SIGIL);

            _generator.Register<Randomizer>();
            _generator.Register<Stalk>();
            _generator.Register<Tomato>();
            _generator.Register<Vote>();
            _generator.Register<Counter>();
            _generator.Register<Deleter>();
            _generator.Register<History>();

            _discord = _generator.Client;

            _LOG.Info($"Sigil is {_generator.Sigil}");
            _LOG.Info("Awaiting commands");

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
}