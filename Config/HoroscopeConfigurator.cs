using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

/// <summary>
///     See <see cref="Horoscope" />.
///     Configures the Horoscope Links variables by fetching the relevant information.
/// </summary>
public class HoroscopeConfigurator
{
    private readonly string _path;

    public HoroscopeConfigurator(string path)
    {
        _path = path;
    }

    public record Horoscope(List<string> links, List<HoroscopeSign> signs);
    public record HoroscopeSign(string sign, List<string> aliases);

    public async Task<Horoscope> ReadHoroscopeAsync()
    {
        using var sr = File.OpenText(_path);
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.Latin1Supplement)
        };
        return await JsonSerializer.DeserializeAsync<Horoscope> (sr.BaseStream, options) ?? new Horoscope(new List<string>(), new List<HoroscopeSign>());
    }
}
