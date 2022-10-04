using System.Threading.Tasks;
using Bishop.Helper.Extensions;
using Bishop.Helper.Grive;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bishop.Commands.Dump;

public class Piggies: BaseCommandModule
{
    public GriveWalker Walker { private get; set; } = null!;
    
    [Command("piggy")]
    [Aliases("pig")]
    [Description("Sends a random pigture.")]
    public async Task Random(CommandContext context)
    {
        await context.RespondAsync(Walker.GetFiles(GriveDirectory.Pigtures).Random() ?? "<null>");
    }
    
}