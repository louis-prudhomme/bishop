using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands
{
    public class Fukup : BaseCommandModule
    {
        public RecordRepository RecordRepository { private get; set; }

        [Command("fukup")]
        public async Task FixMe(CommandContext command, DiscordMember member)
        {
            var id = member.Id;
            
            var categories = await Enumerat.FindAllWithHistoryAsync(member);
            var records = categories
                .Select(enumerat => (enumerat.Key, enumerat.History))
                .Select(tuple => tuple.History
                    .Select(record => new RecordEntity(id, tuple.Key, record)))
                .SelectMany(entities => entities);
            
            await RecordRepository.InsertManyAsync(records);
            await command.RespondAsync("Finished");
        }
    }
}