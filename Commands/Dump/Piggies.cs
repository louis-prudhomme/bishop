using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;
using Bishop.Helper.Grive;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Bishop.Commands.Dump;

public class Piggies: BaseCommandModule
{
    public GriveWalker Walker { private get; set; } = null!;
    
    [Command("piggy")]
    [Aliases("pig")]
    [Description("Sends a random pigture.")]
    public async Task Random(CommandContext context)
    {
        var p = Walker.GetFiles(GriveDirectory.Pigtures)!.Random();
        if (p == null) return;
        var z = new DiscordMessageBuilder();
        await using var f = File.Open(p, FileMode.Open);
        z.WithFile(f);
        await context.RespondAsync(z);
    }
    
}