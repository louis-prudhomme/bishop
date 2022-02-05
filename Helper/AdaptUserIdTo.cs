using System;
using System.Threading.Tasks;

namespace Bishop.Helper;

public class AdaptUserIdTo
{
    public static Func<ulong, Task<string>> UserNameAsync { get; set; } = null!;
}