using System;
using System.Threading.Tasks;

using Commands;
using Config;
using System.Net.Sockets;
using Bishop.Config;

namespace Bishop
{
    class Program
    {
        private static readonly string _token = Environment
            .GetEnvironmentVariable("DISCORD_TOKEN");
        private static readonly string _tomatoesFilePath = Environment
            .GetEnvironmentVariable("TOMATOES_FILE");
        private static readonly string _fkinHerokuPort = Environment
            .GetEnvironmentVariable("PORT");

        private static DiscordClient _discord;
        private static DiscordClientGenerator _generator;

        [STAThread]
        static void Main(string[] args)
        {
            Tomato.Tomatoes = new TomatoConfigurator(_tomatoesFilePath)
                .ReadTomatoesAsync()
                .Result;

            _generator = new DiscordClientGenerator(_token);
            _generator.Commands.RegisterCommands<Tomato>();
            _discord = _generator.Client;

            HerokuConfigurator.Herocul(_fkinHerokuPort);

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
