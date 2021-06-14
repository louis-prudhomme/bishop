using System;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Collections.Generic;

namespace Commands
{
    public class Tomato : BaseCommandModule
    {
        public static List<string> Tomatoes;
        private static Random _rand = new Random();

        [Command("tomato"), Aliases("t")]
        [Description("Throw a tomato to @someone")]
        public async Task Throw(CommandContext context, [Description("User to throw the tomato at!")] DiscordMember member)
        {
            await context.RespondAsync($"{member.Mention} 🍅 ! {Tomatoes[_rand.Next(Tomatoes.Count)]}");
        }
    }
}