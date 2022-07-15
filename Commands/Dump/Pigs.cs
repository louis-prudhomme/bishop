using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Config;
using Bishop.Helper.Grive;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

public class Pigs : BaseCommandModule
{
    private static readonly TimeSpan CacheLifeExpectancy = new(1, 0, 0, 0);

    private static IList<SimpleFile> _pigtures = null!;
    private static DateTime _updateAfter = DateTime.MinValue;

    public Grive Grive { private get; set; } = null!;
    private readonly Random _rand = new();

    [Command("piggy")]
    [Aliases("oink", "gruik", "huiiiii")]
    public async Task Oink(CommandContext context)
    {
        try
        {
            if (_updateAfter.CompareTo(DateTime.Now) <= 0)
                await RebuildCache();
            
            var file = await Grive.FetchCompleteFile(_pigtures[_rand.Next(_pigtures.Count)]);

            if (file == null) await context.RespondAsync("File was not found");

            await context.RespondAsync($"{file?.ThumbnailLink}");
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }

    private async Task RebuildCache()
    {
        _pigtures = await Grive.FetchAllFiles(GriveFolderRegistry.Pigture,
            MimeTypes.Jpeg, MimeTypes.Png);
        _updateAfter = DateTime.Now.Add(CacheLifeExpectancy);
    }
}