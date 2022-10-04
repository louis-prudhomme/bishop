using System;
using System.Threading.Tasks;

namespace Bishop.Helper;

public static class UserNameAccessor
{
    public static long CacheForSeconds = long.MaxValue;
    public static Func<ulong, Task<string>> FetchUserName { private get; set; } = null!;
    public static string FetchUserNameSync(ulong id) => FetchUserName(id).Result;
}