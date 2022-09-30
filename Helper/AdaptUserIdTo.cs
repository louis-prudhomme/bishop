using System;
using System.Threading.Tasks;

namespace Bishop.Helper;

public static class AdaptUserIdTo
{
    public static Func<ulong, Task<string>> UserNameAsync { get; set; } = null!;
}