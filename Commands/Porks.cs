using System;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Grive;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands
{
    /// <summary>
    ///     This class provides a set of commands to delete messages.
    /// </summary>
    internal class Porks : BaseCommandModule
    {
        private readonly PigturesGriveManager PigturesGriveManager = new PigturesGriveManager();

        [Command("porks")]
        [Aliases("p")]
        [Description("Displays wonderful pictures of beautiful pigs !")]
        public async Task GetPictureById(CommandContext context, [Description("Identifier of the pork")] int id)
        {
            try
            {
                await context.RespondAsync((await PigturesGriveManager.FetchPig(id)).Name);
            }
            catch (Exception e)
            {
                await context.RespondAsync(e.Message);
            }
        }
    }
}