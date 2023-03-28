using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;


using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.CardGame;

/// <summary>
///     This class provides a set of commands to track car games owned by Vayames.
/// </summary>
[SlashCommandGroup("cardGame", "Card game tracking commands")]
internal class CardGameService : ApplicationCommandModule
{
    public CardGameRepository Repository { private get; set; } = null!;
    public CardGameFormatter Formatter { private get; set; } = null!;

    [SlashCommand("context", "Prompts all card games owned by Vayames.")]
    public async Task Prompt(InteractionContext context)
    {
        var cardGames = await Repository.FindAllAsync();
        var trueLimit = cardGames.Count; // FIXME: paginate

        if (cardGames.Count == 0)
        {
            await context.CreateResponseAsync("No cards in the collection (weird).");
            return;
        }

        var formattedCardGames = await Task.WhenAll(cardGames
            .Take(trueLimit)
            .Select(Formatter.Format));
        var answer = formattedCardGames
            .Prepend($"The collection currently counts *{cardGames.Count}* card games :")
            .ToList();

        await context.CreateResponseAsync(answer);
    }

    [SlashCommand("add", "Adds a card game to the collection in the name of provided user.")]
    public async Task AddFrom(InteractionContext context,
        [OptionAttribute("gifter", "User offering the card game")]
        DiscordUser gifter,
        [OptionAttribute("Name", "Name of the card game")]
        string cardGameName)
    {
        var newCardGame = new CardGameEntity(cardGameName, gifter.Id);

        await Repository.SaveAsync(newCardGame);

        await context.CreateResponseAsync($"*{cardGameName}* was added to the collection by **{gifter.Mention}** !");
    }

    [SlashCommand("gift", "Adds a card game to the collection in your name.")]
    public async Task Add(InteractionContext context,
        [OptionAttribute("Name", "Name of the card game")]
        string cardGameName)
    {
        if (context.Member != null) await AddFrom(context, context.Member, cardGameName);
        else await context.CreateResponseAsync("Wtf happened ?!");
    }
}