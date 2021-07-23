using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands
{
    public class Stalk : BaseCommandModule
    {
        private const string PATH = "./Resources/Slenders.png";
        public static Dictionary<string, string> Lines { get; set; }

        [Command("stalk")]
        [Aliases("st")]
        [Description("Invoke a discussion betweden you and one of the five Slenders")]
        public async Task Roast(CommandContext context)
        {
            await context.RespondAsync(new DiscordMessageBuilder().WithFile(new FileStream(PATH, FileMode.Open)));
        }

        [Command("stalk")]
        [Description("Invoke a discussion betweden you and one of the five Slenders")]
        public async Task Roast(CommandContext context, [RemainingText] string name)
        {
            await context.RespondAsync(Lines[name]);
        }
    }
}