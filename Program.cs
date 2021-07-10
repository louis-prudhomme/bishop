using System;
using System.Threading.Tasks;

using DSharpPlus;

using Commands;
using Config;
using Bishop.Commands;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Bishop
{
    class Program
    {
        private static readonly string TOMATO_FILE_PATH = "./Resources/tomatoes.json";
        private static readonly string STALK_FILE_PATH = "./Resources/slenders.json";
        private static readonly string _token = Environment
            .GetEnvironmentVariable("DISCORD_TOKEN");

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

            _generator = new DiscordClientGenerator(_token);

            _generator.Register<Randomizer>();
            _generator.Register<Stalk>();
            _generator.Register<Tomato>();
            _generator.Register<Vote>();

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
