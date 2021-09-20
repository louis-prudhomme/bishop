using System;
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
        public async Task Score(CommandContext context,
            [Description("Target key (must be Sauce/Sel/BDM)")]
            MeterCategories meterCategory,
            [Description("Target @user")] DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, meterCategory, nb);
        }

        [Command("score")]
        public async Task Score(CommandContext context,
            [Description("Target key (must be Sauce/Sel/BDM)")]
            MeterCategories meterCategory,
            [Description("Target @user")] DiscordMember member)
        {
            await Score(context, member, meterCategory);
        }

        [Command("sel")]
        //[Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to increment the sel score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Sel, nb);
        }

        [Command("sauce")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to increment the sauce score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Sauce, nb);
        }

        [Command("bdm")]
        //[Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to increment the BDM score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Bdm, nb);
        }

        [Command("beauf")]
        //[Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBeauf(CommandContext context,
            [Description("User to increment the beauf score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Score(context, member, MeterCategories.Beauf, nb);
        }

        [Command("sel")]
        //[Description("Returns the value of @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Sel);
        }

        [Command("sauce")]
        //[Description("Returns the value of @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Sauce);
        }

        [Command("bdm")]
        //[Description("Returns the value of @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the BDM score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Bdm);
        }

        [Command("beauf")]
        //[Description("Returns the value of @someone’s BDM score")]
        public async Task ScoreBeauf(CommandContext context,
            [Description("User to know the beauf score of")]
            DiscordMember member)
        {
            await Score(context, member, MeterCategories.Beauf);
        }

        [Command("sel")]
        //[Description("Returns all sel scores")] todo
        public async Task ScoreSel(CommandContext context)
        {
            await Score(context, MeterCategories.Sel);
        }

        [Command("sauce")]
        //[Description("Returns all sauce scores")]
        public async Task ScoreSauce(CommandContext context)
        {
            await Score(context, MeterCategories.Sauce);
        }

        [Command("bdm")]
        //[Description("Returns all BDM scores")]
        public async Task ScoreBdm(CommandContext context)
        {
            await Score(context, MeterCategories.Bdm);
        }

        [Command("beauf")]
        //[Description("Returns all BDM scores")]
        public async Task ScoreBeauf(CommandContext context)
        {
            await Score(context, MeterCategories.Beauf);
        }

        [Command("sel")]
        //[Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to increment the sel score of by 1")]
            DiscordMember member,
            [Description("Reason for the increment")]
            string reason)
        {
            await Score(context, member, MeterCategories.Sel, reason);
        }

        [Command("sauce")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to increment the sauce score of by 1")]
            DiscordMember member,
            [Description("Reason for the increment")]
            string reason)
        {
            await Score(context, member, MeterCategories.Sauce, reason);
        }

        [Command("bdm")]
        //[Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to increment the BDM score of by 1")]
            DiscordMember member,
            [Description("Reason for the increment")]
            string reason)
        {
            await Score(context, member, MeterCategories.Bdm, reason);
        }

        [Command("beauf")]
        //[Description("Adds a provided value to @someone’s BDM score")]
        public async Task ScoreBeauf(CommandContext context,
            [Description("User to increment the beauf score of by 1")]
            DiscordMember member,
            [Description("Reason for the increment")]
            string reason)
        {
            await Score(context, member, MeterCategories.Beauf, reason);
        }
    }
}