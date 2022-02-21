using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bishop.Commands.Dump;

namespace Bishop.Config;

/// <summary>
///     See <see cref="Aled" />.
///     Configures the Aleds variables by fetching the relevant information.
/// </summary>
public class AledConfigurator
{
    private readonly string _path;

    public AledConfigurator(string path)
    {
        _path = path;
    }

    public async Task<List<string>> ReadAledsAsync()
    {
        using var sr = File.OpenText(_path);
        return await JsonSerializer.DeserializeAsync<List<string>>(sr.BaseStream) ?? new List<string>();
    }
}