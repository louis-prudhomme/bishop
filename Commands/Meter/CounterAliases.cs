using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter
{
    internal partial class Counter
    {
        [Command("score")]
        [Description("Adds a provided value to @someone’s score")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
            Keys key,
            [Description("User to know the score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, key, nb);
        }

        [Command("score")]
        [Description("Returns @someone’s score for a specific key")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
            Keys key,
            [Description("@User to know the score of")]
            DiscordMember member)
        {
            await Score(context, member, key);
        }

        [Command("sel")]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, Keys.Sel, nb);
        }

        [Command("sauce")]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, Keys.Sauce, nb);
        }

        [Command("bdm")]
        [Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the BDM score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, Keys.Bdm, nb);
        }

        [Command("sel")]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")]
            DiscordMember member)
        {
            await Score(context, member, Keys.Sel);
        }

        [Command("sauce")]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")]
            DiscordMember member)
        {
            await Score(context, member, Keys.Sauce);
        }

        [Command("bdm")]
        [Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the BDM score of")]
            DiscordMember member)
        {
            await Score(context, member, Keys.Bdm);
        }
    }
}