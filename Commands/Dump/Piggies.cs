using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using Bishop.Helper.Grive;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Commands.Dump;

public class Piggies : ApplicationCommandModule
{
    public IKeyBasedCache<GriveDirectory, ImmutableList<string>> FilesCache { private get; set; } = null!;

    [SlashCommand("piggy", "Sends a random pigture.")]
    public async Task Random(InteractionContext context)
    {
        var cache = await FilesCache.Get(GriveDirectory.Pigtures);
        var cachedFiles = cache.Value;
        if (cachedFiles == null || cachedFiles.Count == 0)
        {
            await context.CreateResponseAsync("No piggies could be retrieved :'(");
            return;
        }

        var builder = new DiscordInteractionResponseBuilder();
        await using var piggy = File.Open(cachedFiles.Random()!, FileMode.Open);
        builder.AddFile(piggy);
        await context.CreateResponseAsync(builder);
    }
}