using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Bishop.Helper.Extensions;

public static class InteractionContextAdditions
{
    //FIXME: i do not work as of right now
    public static async Task CreateResponseAsync(this InteractionContext context, List<string> content)
    {
        await context.CreateResponseAsync(content.First());
    }
}