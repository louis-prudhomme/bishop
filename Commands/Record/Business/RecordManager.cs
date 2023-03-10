using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using Bishop.Helper.Database;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Business;

public class RecordManager
{
    public RecordRepository Repository { private get; set; } = new();

    public async Task<List<RecordEntity>> GetAllNonNulls() => (await Repository.FindAllAsync())
        .Where(record => record.Motive != null)
        .ToList();

    public async Task<List<RecordEntity>> Find(ulong userId) => (await Repository.FindByUser(userId))
        .Where(record => record.Motive != null)
        .ToList();

    public async Task<List<RecordEntity>> Find(ulong userId, CounterCategory category) => (await Repository.FindByUserAndCategory(userId, category))
        .Where(record => record.Motive != null)
        .ToList();

    public async Task<List<RecordEntity>> Find(CounterCategory category) => (await Repository.FindByCategory(category))
        .Where(record => record.Motive != null)
        .ToList();

    public async Task<long> FindScore(ulong memberId, CounterCategory category)
    {
        return (await Repository.CountByUserGroupByCategory(memberId)).GetValueOrDefault(category);
    }

    public async Task<Dictionary<CounterCategory, long>> FindScores(ulong memberId)
    {
        return await Repository.CountByUserGroupByCategory(memberId);
    }

    public async Task<List<(ulong UserId, long Score)>> FindScores(CounterCategory category)
    {
        return (await Repository.CountByCategoryGroupByUser(category))
            .Select(pair => (UserId: pair.Key, Score: pair.Value))
            .ToList();
    }

    public List<(TKeyType Key, long Score, int Ranking)> RankScores<TKeyType>(IEnumerable<(TKeyType, long Score)> toRank)
    {
        return toRank.OrderByDescending(pair => pair.Score)
            .Select((pair, i) => (pair.Item1, Value: pair.Score, Ranking: i))
            .ToList();
    }

    public async Task<long> Count(ulong userId, CounterCategory category)
    {
        return await Repository.CountByUserAndCategory(userId, category);
    }

    public record SaveRecordResponse(long PreviousScore, long CurrentScore, long NextMilestone);

    public List<RecordEntity> CreateGhostRecords(SnowflakeObject member, CounterCategory category, int nb)
    {
        return Enumerable
            .Range(0, nb)
            .Select(_ => new RecordEntity(member.Id, category, null))
            .ToList();
    }

    public async Task<SaveRecordResponse> Save(ulong userId, CounterCategory category, List<RecordEntity> toSave)
    {
        var previous = await Count(userId, category);

        if (toSave.Count == 1) await Repository.SaveAsync(toSave.First());
        else await Repository.InsertManyAsync(toSave);

        var current = previous + toSave.Count;
        return new SaveRecordResponse(previous, current, GetNextMilestone(previous));
    }

    private static long GetNextMilestone(long current)
    {
        return current switch
        {
            < 10 => 10,
            < 50 => 50,
            < 69 => 69,
            < 100 => 100,
            < 666 => 666,
            _ => (current / 100 + 1) * 100
        };
    }
}