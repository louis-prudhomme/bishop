using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bishop.Helper;

/// <summary>
///     It allows us to persist the data using serialization;
/// </summary>
/// <typeparam name="T">entity to deserialize</typeparam>
public class JsonDeserializer<T>
{
    private const string BasePath = "./Resources/";
    private readonly string _filename;

    public JsonDeserializer(string filename)
    {
        _filename = filename;
    }

    public async Task<T> Get()
    {
        await using var sr = File.Open(BasePath + _filename, FileMode.Open);
        var entity = await JsonSerializer.DeserializeAsync<T>(sr);

        if (entity == null) throw new Exception("Shouldn't be null");

        return entity;
    }
}