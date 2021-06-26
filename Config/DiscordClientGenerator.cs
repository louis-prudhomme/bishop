using System;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using DSharpPlus.CommandsNext.Converters;

namespace Config 
{
    public class DiscordClientGenerator
    {
        private static readonly string[] PREFIX = new[] { ":" };

        private readonly string _token;
        private readonly DiscordClient _client;

        private readonly CommandsNextExtension _commands;

        public DiscordClientGenerator(string token)
        {
            _token = token;
            _client = new DiscordClient(AssembleConfig());

            _commands = _client.UseCommandsNext(AssembleCommands());
            _commands.SetHelpFormatter<DefaultHelpFormatter>();
        }

        private CommandsNextConfiguration AssembleCommands()
        {
            return new CommandsNextConfiguration()
            {
                StringPrefixes = PREFIX
            };
        }

        private DiscordConfiguration AssembleConfig()
        {
            return new DiscordConfiguration()
            {
                Token = _token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            };
        }

        public void Register<T>() where T : BaseCommandModule
        {
            _commands.RegisterCommands<T>();
        }

        public DiscordClient Client { get { return _client; } }
    }
}