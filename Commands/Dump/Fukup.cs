using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

/// <summary>
///     This class provides commands to migrate oldish <see cref="Enumerat" /> to newly created entities.
/// </summary>
[Group("fukup")]
[Aliases("fu")]
public class Fukup : BaseCommandModule
{
    public RecordRepository RecordRepository { private get; set; }
    public CounterRepository CounterRepository { private get; set; }

    [Command("records")]
    [Aliases("r")]
    [Description("Tool to migrate records")]
    public async Task FixMeRecords(CommandContext command, DiscordMember member)
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

    [Command("counters")]
    [Aliases("c")]
    [Description("Tool to migrate counters")]
    public async Task FixMeCounters(CommandContext command, DiscordMember member)
    {
        var id = member.Id;

        var categories = await Enumerat.FindAllAsync(member);
        var counters = categories.Select(enumerat => new CounterEntity(id, enumerat));

        await CounterRepository.InsertManyAsync(counters);
        await command.RespondAsync("Finished");
    }
}