using System;
using System.Threading.Tasks;

using DSharpPlus;

using Commands;
using Config;
using Bishop.Commands;
using log4net;
using System.Reflection;
using log4net.Config;
using Bishop.Commands.Meter;

namespace Bishop
{
    class Program
    {
        private static readonly string TOMATO_FILE_PATH = "./Resources/tomatoes.json";
        private static readonly string STALK_FILE_PATH = "./Resources/slenders.json";

        private static readonly string _discordToken = Environment
            .GetEnvironmentVariable("DISCORD_TOKEN");
        private static readonly string _mongoToken = Environment
            .GetEnvironmentVariable("MONGO_TOKEN");
        private static readonly string _mongoDatabase = Environment
            .GetEnvironmentVariable("MONGO_DB");

        private static DiscordClient _discord;
        private static DiscordClientGenerator _generator;

        private static readonly ILog _log = LogManager
            .GetLogger(MethodBase.GetCurrentMethod()
            .DeclaringType);

        [STAThread]
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Tomato.Tomatoes = new TomatoConfigurator(TOMATO_FILE_PATH)
                .ReadTomatoesAsync()
                .Result;

            Stalk.Lines = new StalkConfigurator(STALK_FILE_PATH)
                .ReadStalkAsync()
                .Result;

            Enumerat.Database = _mongoDatabase;
            Enumerat.Mongo = new MongoDB.Driver.MongoClient(_mongoToken);
            _generator = new DiscordClientGenerator(_discordToken);

            _generator.Register<Randomizer>();
            _generator.Register<Stalk>();
            _generator.Register<Tomato>();
            _generator.Register<Vote>();
            _generator.Register<Counter>();
            _generator.Register<Censor>();

            _discord = _generator.Client;

            _log.Info("Awaiting commands");

            MainAsync()
                .GetAwaiter()
                .GetResult();
        }

        static async Task MainAsync()
        {
            await _discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
