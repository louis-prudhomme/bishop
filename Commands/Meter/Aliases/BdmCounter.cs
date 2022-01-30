using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases
{
    [Group("bdm")]
    [Description("Bdm-related commands")]
    public class BdmCounter : BaseCommandModule
    {
        public Counter Counter { private get; set; }
        
        [GroupCommand]
        [Description("Adds a provided value to @someone’s bdm score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to increment the bdm score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Counter.Score(context, member, MeterCategories.Bdm, nb);
        }

        [GroupCommand]
        [Description("Returns the value of @someone’s bdm score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to know the bdm score of")]
            DiscordMember member)
        {
            await Counter.Score(context, member, MeterCategories.Bdm);
        }

        [GroupCommand]
        [Description("Returns all bdm scores")]
        public async Task ScoreBdm(CommandContext context)
        {
            await Counter.Score(context, MeterCategories.Bdm);
        }

        [GroupCommand]
        [Description("Adds a provided value to @someone’s bdm score")]
        public async Task ScoreBdm(CommandContext context,
            [Description("User to increment the bdm score of by 1")]
            DiscordMember member,
            [RemainingText, Description("Reason for the increment")]
            string reason)
        {
            await Counter.Score(context, member, MeterCategories.Bdm , reason);
        }
    }
}