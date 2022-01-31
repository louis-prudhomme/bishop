using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases
{
    [Group("rass")]
    [Aliases("n")]
    [Description("Rass-related commands")]
    public class RassCounter : BaseCommandModule
    {
        public Counter Counter { private get; set; }

        [GroupCommand]
        [Description("Adds a provided value to @someone’s rass score")]
        public async Task ScoreRass(CommandContext context,
            [Description("User to increment the rass score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Counter.Score(context, member, CountCategory.Rass, nb);
        }

        [GroupCommand]
        [Description("Returns the value of @someone’s rass score")]
        public async Task ScoreRass(CommandContext context,
            [Description("User to know the rass score of")]
            DiscordMember member)
        {
            await Counter.Score(context, member, CountCategory.Rass);
        }

        [GroupCommand]
        [Description("Returns all rass scores")]
        public async Task ScoreRass(CommandContext context)
        {
            await Counter.Score(context, CountCategory.Rass);
        }

        [GroupCommand]
        [Description("Adds a provided value to @someone’s rass score")]
        public async Task ScoreRass(CommandContext context,
            [Description("User to increment the rass score of by 1")]
            DiscordMember member,
            [RemainingText] [Description("Reason for the increment")]
            string reason)
        {
            await Counter.Score(context, member, CountCategory.Rass, reason);
        }
    }
}