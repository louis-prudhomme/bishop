using System;
using System.Configuration;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;

using Commands;
using Config;
using GiphyDotNet.Manager;

namespace Bishop
{
    class Program
    {
        private static readonly DiscordConfigGenerator _configGenerator = new DiscordConfigGenerator(ConfigurationManager.AppSettings.Get("discord-token"));
        private static readonly string _tomatoesFilePath = ConfigurationManager.AppSettings.Get("tomatoes");

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

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
