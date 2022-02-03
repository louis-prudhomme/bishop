using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using Bishop.Commands.Meter.Aliases;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Meter;

/// <summary>
///     The <c>Counter</c> class provides a set of commands to keep trace of user's deeds.
///     This file contains all the general and generic commands.
///     Classes specific to each category exist (ex: <see cref="SelCounter" />).
/// </summary>
[Group("score")]
[Aliases("s")]
[Description(
    "Allows interaction with @users’ scores. The scores can be seen by key or by @user, " +
    "and it is possible to add points to a player in a certain category. " +
    "It is also possible to add a reason for the point, which will then be in the @user’s history.")]
public class CounterService : BaseCommandModule
{
    public CounterRepository CounterRepository { private get; set; } = new();

    [GroupCommand]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member)
    {
        var scores = await CounterRepository.FindByUser(member.Id);
        var mapper = UserIdToUserMentionMapper.GetMapperFor(context);

        if (!scores.Any())
            await context.RespondAsync($"No scores for user {member.Username}");
        else
            await context.RespondAsync(scores
                .Select(entity => entity.ToString(mapper))
                .Aggregate((key1, key2) => string.Join("\n", key1, key2)));
    }

    [GroupCommand]
    public async Task Score(CommandContext context,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory counterCategory)
    {
        var scores = await CounterRepository.FindByCategory(counterCategory);
        var mapper = UserIdToUserMentionMapper.GetMapperFor(context);

        if (!scores.Any())
            await context.RespondAsync($"No scores for category {counterCategory}");
        else
            await context.RespondAsync(scores
                .Select(game => game.ToString(mapper))
                .Aggregate((key1, key2) => string.Join("\n", key1, key2)));
    }

    [GroupCommand]
    public async Task Score(CommandContext context,
        [Description("Target @user")] DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory counterCategory)
    {
        var score = await CounterRepository.FindOneByUserAndCategory(member.Id, counterCategory)
                    ?? new CounterEntity(member.Id, counterCategory);
        var mapper = UserIdToUserMentionMapper.GetMapperFor(context);

        await context.RespondAsync(score.ToString(mapper));
    }

    [GroupCommand]
    public async Task Score(CommandContext context,
        [Description("User to add some score to")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory counterCategory,
        [Description("To increment by")] long nb)
    {
        var record = await CounterRepository
                         .FindOneByUserAndCategory(member.Id, counterCategory)
                     ?? new CounterEntity(member.Id, counterCategory);
        var mapper = UserIdToUserMentionMapper.GetMapperFor(context);

        var previous = record.Score;
        record.Score += nb;

        await CounterRepository.SaveAsync(record);
        await context.RespondAsync($"{record.ToString(mapper)} (from {previous})");
    }

    [GroupCommand]
    public async Task Score(CommandContext context,
        [Description("User to increment score of")]
        DiscordMember member,
        [Description("Target key (must be BDM/Beauf/Sauce/Sel/Rass)")]
        CounterCategory counterCategory,
        [RemainingText] [Description("Context for the point(s) addition")]
        string motive)
    {
        var historyService = new RecordService();
        await Task.WhenAll(
            Score(context, member, counterCategory, 1),
            historyService.Add(context, member, counterCategory, motive));
    }
}