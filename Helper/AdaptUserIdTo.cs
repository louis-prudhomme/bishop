using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace Bishop.Helper;

public class AdaptUserIdTo
{
    public static Func<ulong, Task<string>> UserNameAsync { get; set; } = null!;
}