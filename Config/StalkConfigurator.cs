using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bishop.Commands;

namespace Bishop.Config
{
    /// <summary>
    /// Configures the Stalker variables by fetching the relevant information.
    ///
    /// See <see cref="Stalk"/>.
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
            return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(sr.BaseStream);
        }
    }
}