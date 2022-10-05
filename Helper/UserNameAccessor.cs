using System;
using System.Threading.Tasks;

namespace Bishop.Helper;

public static class UserNameAccessor
{
    public const long CacheForSeconds = long.MaxValue;
    public static Func<ulong, Task<string>> FetchUserName { get; set; } = null!;
}