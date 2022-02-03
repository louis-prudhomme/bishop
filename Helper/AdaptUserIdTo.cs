using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace Bishop.Helper;

public class AdaptUserIdTo
{
    //TODO clean this
    [Obsolete("will be cleaned up")]
    public static Func<ulong, string> UserMention { get; set; } = null!;
    //TODO clean this
    [Obsolete("will be cleaned up")]
    public static Func<ulong, string> UserName { get; set; } = null!;
}