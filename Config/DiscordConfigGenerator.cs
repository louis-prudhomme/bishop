using System;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

using DSharpPlus;

namespace Config 
{
    public class DiscordConfigGenerator 
    {
        private string _token;

        public DiscordConfigGenerator(string token)
        {
            _token = token;
        }

        public DiscordConfiguration Configuration 
        {
            get 
            {
                return new DiscordConfiguration() 
                {
                    Token = _token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.AllUnprivileged
                };
            }
        }
    }
}