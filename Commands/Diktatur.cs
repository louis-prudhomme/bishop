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
    public class Diktatur : BaseCommandModule
    {
        private static Random _rand = new Random();

        [Command("diktat"), Aliases("d", "rand", "r")]
        [Description("Make a random choice")]
        public async Task Diktat(CommandContext context, params string[] args)
        {
            await context.RespondAsync($"🎲 ⇒ {args[_rand.Next(args.Length)]}");
        }
    }
}
