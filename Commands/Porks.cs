using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Grive;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands
{
    /// <summary>
    ///     This class provides a set of commands to delete messages.
    /// </summary>
    internal class Porks : BaseCommandModule
    {
        //TODO build cache of pork ids
        //TODO make this work (download files ?)
        private readonly PigturesGriveManager _pigturesGriveManager = new();

        [Command("porks")]
        [Aliases("p")]
        [Description("Displays wonderful pictures of beautiful pigs !")]
        public async Task GetPictureById(CommandContext context, [Description("Identifier of the pork")] int id)
        {
            await context.RespondAsync((await _pigturesGriveManager.FetchPig(id)).WebViewLink);
        }
    }
}