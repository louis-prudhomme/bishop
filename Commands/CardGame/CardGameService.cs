using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;
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
    public CardGameFormatter Formatter { private get; set; } = null!;

    [GroupCommand]
    [Description("Prompts all card games owned by Vayames.")]
    public async Task Prompt(CommandContext context,
        [Description("How many decks do you want to prompt")]
        int? limit = 0)
    {
        var cardGames = await Repository.FindAllAsync();
        var trueLimit = limit <= 0 ? cardGames.Count : limit ?? 0;

        if (cardGames.Count == 0)
        {
            await context.RespondAsync("No cards in the collection (weird).");
            return;
        }

        var formattedCardGames = await Task.WhenAll(cardGames
                .Take(trueLimit)
                .Select(Formatter.Format));
        var answer = formattedCardGames
            .Prepend($"The collection currently counts *{cardGames.Count}* card games :")
            .ToList();

        await context.RespondAsync(answer);
    }

    [Command("add")]
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

    [Command("gift")]
    [Description("Adds a card game to the collection in your name.")]
    public async Task Add(CommandContext context,
        [Description("Name of the card game")] [RemainingText]
        string cardGameName)
    {
        if (context.Member != null) await AddFrom(context, context.Member, cardGameName);
        else await context.RespondAsync("Wtf happened ?!");
    }
}