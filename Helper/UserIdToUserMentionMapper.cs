using System;
using System.Collections.Generic;
using DSharpPlus.CommandsNext;

namespace Bishop.Helper;

public class UserIdToUserMentionMapper
{
    public static Func<ulong, string> GetMapperFor(CommandContext context)
    {
        return id =>
        {
            try
            {
                return context.Guild.Members[id].Mention;
            }
            catch (KeyNotFoundException e)
            {
                // replace with l'étranger
                return "unknown";
            }
        };
    }
}