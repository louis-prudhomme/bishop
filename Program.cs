using System;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;

using Commands;
using Config;
using System.Net.Sockets;

namespace Bishop
{
    class Program
    {
        private static readonly DiscordConfigGenerator _configGenerator = new DiscordConfigGenerator(Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
        private static readonly string _tomatoesFilePath = Environment.GetEnvironmentVariable("TOMATOES_FILE");
        private static readonly int _fkinHerokuPort = int.Parse(Environment.GetEnvironmentVariable("PORT"));

        [STAThread]
        static void Main(string[] args)
        {
            Tomato.Tomatoes = new TomatoConfigurator(_tomatoesFilePath)
                .ReadTomatoesAsync()
                .Result;

            MainAsync()
                .GetAwaiter()
                .GetResult();
        }

        static async Task MainAsync() 
        {
            var discord = new DiscordClient(_configGenerator.Configuration);

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration() 
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<Tomato>();

            var l = new TcpListener(_fkinHerokuPort);
            l.Start();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
