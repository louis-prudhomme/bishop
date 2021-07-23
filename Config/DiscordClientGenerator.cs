using System;
using Bishop.Config.Converters;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;

namespace Bishop.Config
{
    public class DiscordClientGenerator
    {
        private static readonly string[] _PREFIX = {";"};

        private readonly CommandsNextExtension _commands;

        private readonly string _token;
        private readonly string[] _sigil;

        public DiscordClientGenerator(string token, string sigil)
        {
            _token = token;
            _sigil = new[] {sigil};
            Client = new DiscordClient(AssembleConfig());

            _commands = Client.UseCommandsNext(AssembleCommands());
            _commands.SetHelpFormatter<DefaultHelpFormatter>();
            _commands.RegisterConverter(new KeysConverter());
        }


        public DiscordClient Client { get; }
        public string Sigil => string.Join(" ", _sigil);


        private CommandsNextConfiguration AssembleCommands()
        {
            return new()
            {
                StringPrefixes = _sigil ?? _PREFIX
            };
        }

        private DiscordConfiguration AssembleConfig()
        {
            return new()
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
    }
}