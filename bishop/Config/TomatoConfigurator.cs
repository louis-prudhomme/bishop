using System;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Config 
{
    public class TomatoConfigurator
    {
        private readonly string _path;

        public TomatoConfigurator(string path)
        {
            _path = path;
        }

        public async Task<List<string>> ReadTomatoesAsync()
        {
            using (StreamReader sr = File.OpenText(_path))
            {
                return await JsonSerializer.DeserializeAsync<List<string>>(sr.BaseStream);
            };
        }
    }
}