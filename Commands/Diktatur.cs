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
        private static readonly string BASE_EMOJI_HEX = "U+1F6";
        private static readonly int MAX_EMOJI_HEX = 644;
        private static readonly string PADDER = "000";
        private static Random _rand = new Random();


        [Command("Random"), Aliases("rand", "r")]
        [Description("Make a random choice")]
        public async Task Diktat(CommandContext context, [Description("Options to choose from")] params string[] args)
        {
            await context.RespondAsync($"🎲 ⇒ {args[_rand.Next(args.Length)]}");
        }

        [Command("Randomoji"), Aliases("randmoji", "rj")]
        [Description("Picks a random emoji")]
        public async Task DiktatEmoji(CommandContext context)
        {
            string emojiCode = BASE_EMOJI_HEX + _rand.Next(0, MAX_EMOJI_HEX).ToString(PADDER);
            
            await context.RespondAsync($"I’ve picked {char.Parse(emojiCode)}");
        }
    }
}
