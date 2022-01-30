using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases
{
    [Group("sauce")]
    [Description("Sauce-related commands")]
    public class SauceCounter : BaseCommandModule
    {
        public Counter Counter { private get; set; }
        
        [GroupCommand]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to increment the sauce score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Counter.Score(context, member, MeterCategory.Sauce, nb);
        }

        [GroupCommand]
        [Description("Returns the value of @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to know the sauce score of")]
            DiscordMember member)
        {
            await Counter.Score(context, member, MeterCategory.Sauce);
        }

        [GroupCommand]
        [Description("Returns all sauce scores")]
        public async Task ScoreSauce(CommandContext context)
        {
            await Counter.Score(context, MeterCategory.Sauce);
        }

        [GroupCommand]
        [Description("Adds a provided value to @someone’s sauce score")]
        public async Task ScoreSauce(CommandContext context,
            [Description("User to increment the sauce score of by 1")]
            DiscordMember member,
            [RemainingText, Description("Reason for the increment")]
            string reason)
        {
            await Counter.Score(context, member, MeterCategory.Sauce , reason);
        }
    }
}