using System;
using System.IO;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Collections.Generic;

namespace Commands
{
    public class Stalk : BaseCommandModule
    {
        private static string path = "./Resources/Slenders.png";
        public static Dictionary<string, string> Lines;

        [Command("stalk"), Aliases("st")]
        [Description("Invoke a discussion betweden you and one of the five Slenders")]
        public async Task Roast(CommandContext context)
        {
            await context.RespondAsync(new DiscordMessageBuilder().WithFile(new FileStream(path, FileMode.Open)));
        }

        [Command("stalk")]
        [Description("Invoke a discussion betweden you and one of the five Slenders")]
        public async Task Roast(CommandContext context, [RemainingText] string name)
        {
            await context.RespondAsync(Lines[name]);
        }
    }
}