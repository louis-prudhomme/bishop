using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands
{
    public class DJ : BaseCommandModule
    {
        private static Random _rand = new Random();

        [Command("Sound"), Aliases("s")]
        [Description("Play a sound")]
        public async Task Soundboard(CommandContext context, [Description("Options to choose from")] params string[] args)
        {
            await context.RespondAsync($"🎲 ⇒ {args[_rand.Next(args.Length)]}");
        }
    }
}
