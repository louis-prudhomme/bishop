using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.CardGame;

/// <summary>
///     This class provides a set of commands to track car games owned by Vayames.
/// </summary>
[Group("cardGame")]
[Aliases("cg")]
[Description("Card game tracking commands")]
internal class CardGameService : BaseCommandModule
{
    public CardGameRepository Repository { private get; set; } = null!;

    [GroupCommand]
    [Description("Prompts all card games owned by Vayames.")]
    public async Task Prompt(CommandContext context)
    {
        var cardGames = await Repository.FindAllAsync();
        var mapper = UserIdToUserMentionMapper.GetMapperFor(context);

        if (cardGames.Count == 0)
        {
            await context.RespondAsync("No cards in the collection (weird).");
            return;
        }

        var formattedCardGames = cardGames
            .Select(game => game.ToString(mapper))
            .Aggregate((key1, key2) => string.Join("\n", key1, key2));

        await context.RespondAsync($"The collection currently counts *{cardGames.Count}* card games :\n{formattedCardGames}");
    }

    [GroupCommand]
    [Description("Adds a card game to the collection in the name of provided user.")]
    public async Task AddFrom(CommandContext context,
        [Description("User offering the card game")]
        DiscordMember gifter,
        [Description("Name of the card game")] [RemainingText]
        string cardGameName)
    {
        var newCardGame = new CardGameEntity(cardGameName, gifter.Id);

        await Repository.SaveAsync(newCardGame);

        await context.RespondAsync($"*{cardGameName}* was added to the collection by **{gifter.Mention}** !");
    }

    [GroupCommand]
    [Description("Adds a card game to the collection in your name.")]
    public async Task Add(CommandContext context,
        [Description("Name of the card game")] [RemainingText]
        string cardGameName)
    {
        await AddFrom(context, context.Member, cardGameName);
    }
}