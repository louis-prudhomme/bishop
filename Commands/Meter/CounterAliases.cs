using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishop.Commands.Meter
{
    partial class Counter
    {
        [Command("score")]
        [Description("Adds a provided value to @someone’s score")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")] Keys key,
            [Description("User to know the score of")] DiscordMember member,
            [Description("To increment by")] int nb)
        {
            await Score(context, member, key, nb);
        }

        [Command("score")]
        [Description("Returns @someone’s score for a specific key")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")] Keys key, 
            [Description("@User to know the score of")] DiscordMember member)
        {
            await Score(context, member, key);
        }

        [Command("sel")]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")] DiscordMember member,
            [Description("To increment by")] int nb)
        {
            await Score(context, member, Keys.SEL, nb);
        }

        [Command("sauce")]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")] DiscordMember member,
            [Description("To increment by")] int nb)
        {
            await Score(context, member, Keys.SAUCE, nb);
        }

        [Command("bdm")]
        [Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the BDM score of")] DiscordMember member,
            [Description("To increment by")] int nb)
        {
            await Score(context, member, Keys.BDM, nb);
        }

        [Command("sel")]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")] DiscordMember member)
        {
            await Score(context, member, Keys.SEL);
        }

        [Command("sauce")]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")] DiscordMember member)
        {
            await Score(context, member, Keys.SAUCE);
        }

        [Command("bdm")]
        [Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the BDM score of")] DiscordMember member)
        {
            await Score(context, member, Keys.BDM);
        }
    }
}
