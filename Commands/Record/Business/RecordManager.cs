using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.Record.Domain;
using DSharpPlus.Entities;

namespace Bishop.Commands.Record.Business;

public class RecordManager
{
    public RecordRepository Repository { private get; set; } = new();

    public async Task<List<RecordEntity>> GetAllNonNulls()
    {
        return (await Repository.FindAllAsync())
            .Where(record => record.Motive != null)
            .ToList();
    }

    public async Task<List<RecordEntity>> Find(ulong userId)
    {
        return (await Repository.FindByUser(userId))
            .Where(record => record.Motive != null)
            .ToList();
    }

    public async Task<List<RecordEntity>> Find(ulong userId, CounterCategory category)
    {
        return (await Repository.FindByUserAndCategory(userId, category))
            .Where(record => record.Motive != null)
            .ToList();
    }

    public async Task<List<RecordEntity>> Find(CounterCategory category)
    {
        return (await Repository.FindByCategory(category))
            .Where(record => record.Motive != null)
            .ToList();
    }

    public async Task<long> FindScore(ulong userId, CounterCategory category)
    {
        return (await Repository.CountByUserGroupByCategory(userId)).GetValueOrDefault(category);
    }

    public async Task<int> FindRank(ulong userId, CounterCategory category)
    {
        return RankScores((await Repository.CountByCategoryGroupByUser(category))
                .Select(pair => (UserId: pair.Key, Score: pair.Value)))
            .First(tuple => tuple.Key == userId)
            .Ranking;
    }

    public async Task<Dictionary<CounterCategory, long>> FindScores(ulong userId)
    {
        return await Repository.CountByUserGroupByCategory(userId);
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

    public List<RecordEntity> CreateGhostRecords(SnowflakeObject user, CounterCategory category, long nb)
    {
        return Enumerable
            .Range(0, unchecked((int)nb))
            .Select(_ => new RecordEntity(user.Id, category, null))
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

    public record SaveRecordResponse(long PreviousScore, long CurrentScore, long NextMilestone);
}
