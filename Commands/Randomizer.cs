using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands
{
    public class Randomizer : BaseCommandModule
    {
        private const int BASE_EMOJI_HEX = 0x1F600;
        private const int MAX_EMOJI_HEX = 44;
        private static readonly Random _RAND = new();


        [Command("Random")]
        [Aliases("rand", "r")]
        [Description("Make a random choice")]
        public async Task RandomChoice(CommandContext context, [Description("Options to choose from")]
            params string[] args)
        {
            await context.RespondAsync($"🎲 ⇒ {args[_RAND.Next(args.Length)]}");
        }

        [Command("Randomoji")]
        [Aliases("randmoji", "rj")]
        [Description("Picks a random emoji")]
        public async Task DiktatEmoji(CommandContext context)
        {
            var emojiCode = BASE_EMOJI_HEX + _RAND.Next(0, MAX_EMOJI_HEX);

            await context.RespondAsync($"I’ve picked : {char.ConvertFromUtf32(emojiCode)}");
        }
    }
}