using Bishop.Config.Converters;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;

namespace Bishop.Config
{
    /// <summary>
    ///     Generates the configuration of the Discord Client.
    /// </summary>
    public class DiscordClientGenerator
    {
        /// <summary>
        ///     Can be overriden by environment variables. See <see cref="Program" />.
        /// </summary>
        private static readonly string[] _PREFIX = {";"};

        private readonly CommandsNextExtension _commands;
        private readonly string[] _sigil;

        private readonly string _token;

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
            return new CommandsNextConfiguration
            {
                StringPrefixes = _sigil ?? _PREFIX
            };
        }

        private DiscordConfiguration AssembleConfig()
        {
            return new DiscordConfiguration
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