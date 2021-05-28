using System;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Commands
{
    public class Tomato : BaseCommandModule
    {
        [Command("tomato"), Aliases("t")]
        public async Task Throw(CommandContext context, DiscordMember member)
        {
            await context.RespondAsync($"{member.Mention} 🍅 !");
        }
    }
}