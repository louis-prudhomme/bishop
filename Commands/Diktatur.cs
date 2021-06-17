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
        private static readonly int BASE_EMOJI_HEX = 0x1F600;
        private static readonly int MAXX_EMOJI_HEX = 44;
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
            int emojiCode = BASE_EMOJI_HEX + _rand.Next(0, MAXX_EMOJI_HEX);

            await context.RespondAsync($"I’ve picked {char.ConvertFromUtf32(emojiCode)}");
        }
    }
}
