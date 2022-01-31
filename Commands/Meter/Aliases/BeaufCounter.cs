using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases
{
    [Group("beauf")]
    [Description("Beauf-related commands")]
    public class BeaufCounter : BaseCommandModule
    {
        public Counter Counter { private get; set; }
        
        [GroupCommand]
        [Description("Adds a provided value to @someone’s beauf score")]
        public async Task ScoreBeauf(CommandContext context,
            [Description("User to increment the beauf score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Counter.Score(context, member, CountCategory.Beauf, nb);
        }

        [GroupCommand]
        [Description("Returns the value of @someone’s beauf score")]
        public async Task ScoreBeauf(CommandContext context,
            [Description("User to know the beauf score of")]
            DiscordMember member)
        {
            await Counter.Score(context, member, CountCategory.Beauf);
        }

        [GroupCommand]
        [Description("Returns all beauf scores")]
        public async Task ScoreBeauf(CommandContext context)
        {
            await Counter.Score(context, CountCategory.Beauf);
        }

        [GroupCommand]
        [Description("Adds a provided value to @someone’s beauf score")]
        public async Task ScoreBeauf(CommandContext context,
            [Description("User to increment the beauf score of by 1")]
            DiscordMember member,
            [RemainingText, Description("Reason for the increment")]
            string reason)
        {
            await Counter.Score(context, member, CountCategory.Beauf , reason);
        }
    }
}