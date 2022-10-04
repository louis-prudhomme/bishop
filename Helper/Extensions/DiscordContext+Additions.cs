using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace Bishop.Helper.Extensions;

public static class CommandContextAdditions
{
    public static async Task RespondAsync(this CommandContext context, List<string> content) => await DiscordMessageCutter.PaginateAnswer(content, context.RespondAsync);
}