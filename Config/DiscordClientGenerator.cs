using System;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace Config 
{
    public class DiscordClientGenerator
    {
        private static readonly string[] PREFIX = new[] { "*" };

        private readonly string _token;
        private readonly DiscordClient _client;

        private readonly CommandsNextExtension _commands;

        public DiscordClientGenerator(string token)
        {
            _token = token;
            _client = new DiscordClient(AssembleConfig());

            _commands = _client.UseCommandsNext(AssembleCommands());
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

        public CommandsNextExtension Commands { get { return _commands; } }

        public DiscordClient Client { get { return _client; } }
    }
}