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
        [Command("stalk"), Aliases("st")]
        [Description("Invoke a discussion between you and one of the five Slenders")]
        public async Task Roast(CommandContext context)
        {
            try
            {
                await context.RespondAsync(new DiscordMessageBuilder().WithFile(new FileStream("./Resources/Randy_Man.jpg", FileMode.Open)));
            }
            catch (Exception e)
            {
                await context.RespondAsync(e.Message);
            }
        }
    }
}