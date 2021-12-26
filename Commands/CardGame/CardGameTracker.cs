using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.CardGame
{
    /// <summary>
    ///     This class provides a set of commands to track car games owned by Vayames.
    /// </summary>
    [Group("cardGame")]
    [Aliases("cg")]
    [Description("Card game tracking commands")]
    internal class CardGameTracker : BaseCommandModule
    {
        [GroupCommand]
        [Description("Prompts all card games owned by Vayames.")]
        public async Task Prompt(CommandContext context)
        {
            var cardGames = await CardCollection.FindAllAsync();

            await context.RespondAsync($"The collection currently counts *{cardGames.Count}* card games.");

            if (cardGames.Count == 0) return;

            await context.RespondAsync(cardGames
                .Select(game => game.ToString())
                .Aggregate((key1, key2) => string.Join("\n", key1, key2)));
        }

        [Command("from")]
        [Aliases("f", ">")]
        [Description("Adds a card game to the collection in the name of provided user.")]

        public async Task AddFrom(CommandContext context,
            [Description("User offering the card game")]
            DiscordMember gifter,
            [Description("Name of the card game")] [RemainingText]
            string cardGameName)
        {
            var newCardgame = new CardGame(cardGameName, gifter.Username);

            await CardCollection.AddAsync(newCardgame);

            await context.RespondAsync($"*{cardGameName}* was added to the collection by **{gifter.Username}** !");
        }
        
        [Command("add")]
        [Aliases("a", "+")]
        [Description("Adds a card game to the collection in your name.")]
        public async Task Add(CommandContext context,
            [Description("Name of the card game")] [RemainingText]
            string cardGameName)
        {
            var newCardgame = new CardGame(cardGameName, context.Member.Username);

            await CardCollection.AddAsync(newCardgame);

            await context.RespondAsync($"*{cardGameName}* was added to the collection !");
        }
    }
}