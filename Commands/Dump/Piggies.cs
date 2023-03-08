using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Bishop.Helper;
using Bishop.Helper.Extensions;
using Bishop.Helper.Grive;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

public class Piggies: BaseCommandModule
{
    public IKeyBasedCache<GriveDirectory, ImmutableList<string>> FilesCache { private get; set; } = null!;
    
    [Command("piggy")]
    [Aliases("pig")]
    [Description("Sends a random pigture.")]
    public async Task Random(CommandContext context)
    {
            var cache = await FilesCache.Get(GriveDirectory.Pigtures);
            var cachedFiles = cache.Value;
            if (cachedFiles == null || cachedFiles.Count == 0)
            {
                await context.RespondAsync("No piggies could be retrieved :'(");
                return;
            }

            var builder = new DiscordMessageBuilder();
            await using var piggy = File.Open(cachedFiles!.Random()!, FileMode.Open);
            builder.WithFile(piggy);
            await context.RespondAsync(builder);
    }
    
}