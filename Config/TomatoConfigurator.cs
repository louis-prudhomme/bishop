using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bishop.Commands;

namespace Bishop.Config
{
    /// <summary>
    ///     Configures the Tomatoes variables by fetching the relevant information.
    ///     See <see cref="Tomato" />.
    /// </summary>
    public class TomatoConfigurator
    {
        private readonly string _path;

        public TomatoConfigurator(string path)
        {
            _path = path;
        }

        public async Task<List<string>> ReadTomatoesAsync()
        {
            using var sr = File.OpenText(_path);
            return await JsonSerializer.DeserializeAsync<List<string>>(sr.BaseStream);
        }
    }
}