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
    internal class CardGameTracker : BaseCommandModule
    {
        [Command("cardGame")]
        [Aliases("cg")]
        [Description("Prompts all card games owned by Vayames.")]
        public async Task Prompt(CommandContext context)
        {
            try
            {
                var cardGames = await CardCollection.FindAllAsync();

                await context.RespondAsync($"The collection currently counts {cardGames.Count} card games.");

                if (cardGames.Count == 0) return;
                
                await context.RespondAsync(cardGames
                    .Select(game => game.ToString())
                    .Aggregate((key1, key2) => string.Join("\n", key1, key2)));
            }
            catch (Exception e)
            {
                await context.RespondAsync(e.Message);
            }
        }
        
        

        [Command("cardGame")]
        public async Task Add(CommandContext context, [RemainingText] string cardGameName)
        {
            var newCardgame = new CardGame(cardGameName, context.Member.Username);

            await CardCollection.AddAsync(newCardgame);

            await context.RespondAsync($"{cardGameName} was added to the collection !");
        }
        
        

        [Command("cardGame")]
        public async Task Add(CommandContext context, 
            [Description("User offering the card game")]
            DiscordMember gifter, 
            [RemainingText] string cardGameName)
        {
            var newCardgame = new CardGame(cardGameName, gifter.Username);

            await CardCollection.AddAsync(newCardgame);

            await context.RespondAsync($"{cardGameName} was added to the collection !");
        }
    }
}