using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using DSharpPlus;

namespace Bishop.Commands
{
    public class Vote : BaseCommandModule
    {
        private static readonly string EMOJI_PREFIX = ":regional_indicator_*:";
        private static readonly char EMOJI_PREFIX_PLACEHOLDER = '*';
        private static readonly string MESSAGE_BASE = "**Aux urnes !**";
        private static readonly int A_ASCII_INDEX = 97;

        [Command("referendum"), Aliases("vote", "v")]
        [Description("Create a poll")]
        public async Task Referendum(CommandContext context, [Description("Options to choose from")] params string[] args)
        {
            var messageBuilder = new StringBuilder(MESSAGE_BASE);

            for (int i = 0; i < args.Length; i++)
                messageBuilder.Append($"\n{args[i]} ⇒ {RegionalIndicatorFromIndex(context.Client, i)}");
            
            var sentMessage = await context.RespondAsync(messageBuilder.ToString());

            for (int i = 0; i < args.Length; i++)
                await sentMessage.CreateReactionAsync(RegionalIndicatorFromIndex(context.Client, i));
        }

        private DiscordEmoji RegionalIndicatorFromIndex(DiscordClient client, int index)
        {
            return DiscordEmoji.FromName(client, EMOJI_PREFIX.Replace(EMOJI_PREFIX_PLACEHOLDER, (char)(A_ASCII_INDEX + index)));
        }
    }
}
