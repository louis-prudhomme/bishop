using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bishop.Commands.Dump;

namespace Bishop.Config;

/// <summary>
///     See <see cref="Quote" />.
///     Configures the Quotes variables by fetching the relevant information.
/// </summary>
public class QuoteConfigurator
{
    private readonly string _path;

    public QuoteConfigurator(string path)
    {
        _path = path;
    }

    public record Politician(string name, List<string> quotes);

    public async Task<List<Politician>> ReadQuotesAsync()
    {
        using var sr = File.OpenText(_path);
        return await JsonSerializer.DeserializeAsync<List<Politician>>(sr.BaseStream) ?? new List<Politician>();
    }
}