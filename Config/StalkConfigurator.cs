using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bishop.Config;

/// <summary>
///     Configures the Stalker variables by fetching the relevant information.
///     See <see cref="Stalk" />.
/// </summary>
public class StalkConfigurator
{
    private readonly string _path;

    public StalkConfigurator(string path)
    {
        _path = path;
    }

    public async Task<Dictionary<string, string>> ReadStalkAsync()
    {
        using var sr = File.OpenText(_path);
        var result = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(sr.BaseStream);

        if (result == null)
            throw new NullReferenceException();

        return result;
    }
}