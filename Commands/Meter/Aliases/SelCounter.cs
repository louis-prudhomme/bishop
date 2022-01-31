using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter.Aliases
{
    [Group("sel")]
    [Description("Sel-related commands")]
    public class SelCounter : BaseCommandModule
    {
        public Counter Counter { private get; set; }
        
        [GroupCommand]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to increment the sel score of")]
            DiscordMember member,
            [Description("To increment by")] long nb)
        {
            await Counter.Score(context, member, CountCategory.Sel, nb);
        }

        [GroupCommand]
        [Description("Returns the value of @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to know the sel score of")]
            DiscordMember member)
        {
            await Counter.Score(context, member, CountCategory.Sel);
        }

        [GroupCommand]
        [Description("Returns all sel scores")]
        public async Task ScoreSel(CommandContext context)
        {
            await Counter.Score(context, CountCategory.Sel);
        }

        [GroupCommand]
        [Description("Adds a provided value to @someone’s sel score")]
        public async Task ScoreSel(CommandContext context,
            [Description("User to increment the sel score of by 1")]
            DiscordMember member,
            [RemainingText, Description("Reason for the increment")]
            string reason)
        {
            await Counter.Score(context, member, CountCategory.Sel, reason);
        }
    }
}