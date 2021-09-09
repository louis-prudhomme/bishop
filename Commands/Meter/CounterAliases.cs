using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter
{
    /// <summary>
    ///     Aliases for the <c>Counter</c> class.
    ///     See <see cref="Counter" />
    /// </summary>
    internal partial class Counter
    {
        [Command("score")]
        [Description("Adds a provided value to @someone’s score")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
            MeterCategories meterCategory,
            [Description("User to know the score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, meterCategory, nb);
        }

        [Command("score")]
        [Description("Returns @someone’s score for a specific key")]
        public async Task Score(CommandContext context,
            [Description("Key to list scores of (must be one of BDM/SAUCE/SEL)")]
            MeterCategories meterCategory,
            [Description("@User to know the score of")]
            DiscordMember member)
        {
            await Score(context, member, meterCategory);
        }

        [Command("sel")]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to increment the sel score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Sel, nb);
        }

        [Command("sauce")]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to increment the sauce score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Sauce, nb);
        }

        [Command("bdm")]
        [Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to increment the BDM score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Bdm, nb);
        }

        [Command("sel")]
        [Description("Returns the value of @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Sel);
        }

        [Command("sauce")]
        [Description("Returns the value of @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Sauce);
        }

        [Command("bdm")]
        [Description("Returns the value of @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the BDM score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Bdm);
        }

        [Command("sel")]
        [Description("Returns all sel scores")]
        public async Task ScoreSel(CommandContext context)
        {
            await Score(context, MeterCategories.Sel);
        }

        [Command("sauce")]
        [Description("Returns all sauce scores")]
        public async Task ScoreSauce(CommandContext context)
        {
            await Score(context, MeterCategories.Sauce);
        }

        [Command("bdm")]
        [Description("Returns all BDM scores")]
        public async Task ScoreBdm(CommandContext context)
        {
            await Score(context, MeterCategories.Bdm);
        }
    }
}