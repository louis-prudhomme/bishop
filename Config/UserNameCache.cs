using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;

namespace Bishop.Config;

public class UserNameCache
{
    private readonly ConcurrentDictionary<ulong, string> _idToName = new();

    public List<ulong> Keys => new(_idToName.Keys);

    public List<(ulong, string)> Stored => _idToName.Select(pair => (pair.Key, pair.Value)).ToList();

    public async Task<string> GetAsync(ulong id)
    {
        if (_idToName.ContainsKey(id))
            return _idToName[id];

        var fetched = await AdaptUserIdTo.UserNameAsync(id);
        _idToName.AddOrUpdate(id, _ => fetched, (_, _) => fetched);
        return fetched;
    }

    public void DirectAdd(ulong id, string userName)
    {
        _idToName.AddOrUpdate(id, _ => userName, (_, _) => userName);
    }

    public void Nuke()
    {
        _idToName.Clear();
    }
}