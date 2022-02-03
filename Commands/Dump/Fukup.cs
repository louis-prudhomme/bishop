using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.CardGame;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

/// <summary>
///     This class provides commands to migrate oldish <see cref="Enumerat" /> & <see cref="CardCollection" /> to newly
///     created entities.
/// </summary>
[Group("fukup")]
[Aliases("fu")]
[RequireOwner]
public class Fukup : BaseCommandModule
{
    public RecordRepository RecordRepository { private get; set; } = null!;
    public CounterRepository CounterRepository { private get; set; } = null!;
    public CardGameRepository CardGameRepository { private get; set; } = null!;

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

    [Command("decks")]
    [Aliases("d")]
    [Description("Tool to migrate card game decks")]
    public async Task FixMeDecks(CommandContext command)
    {
        var oldDecks = await CardCollection.FindAllAsync();

        Func<(string, ulong), ulong> idChanger = (tuple) => tuple.Item1.Equals("PrideTheDaemon")
            ? 258919647984746497u
            : tuple.Item2;  

        var userIdDict = oldDecks.Select(game => game.Gifter)
            .Distinct()
            .Select(name => (name, command.Guild.Members.Values
                .Select(member => (member.Id, member.Username))
                .Where(tuple => tuple.Item2.Equals(name))
                .Select(tuple => tuple.Id)
                .FirstOrDefault()))
            .Select(tuple => (idChanger(tuple), tuple.name))
            .ToDictionary(tuple => tuple.Item2, tuple => tuple.Item1);

        var decks = oldDecks.Select(old => 
            new CardGameEntity(old, userIdDict[old.Gifter]));

        await CardGameRepository.InsertManyAsync(decks);
        await command.RespondAsync("Finished");
    }
}