using System;
using System.Configuration;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;

using Commands;
using Config;

namespace Bishop
{
    class Program
    {
        private static readonly DiscordConfigGenerator _configGenerator = new DiscordConfigGenerator(ConfigurationManager.AppSettings.Get("token"));

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
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
