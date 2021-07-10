using System;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Config 
{
    public class StalkConfigurator
    {
        private readonly string _path;

        public StalkConfigurator(string path)
        {
            _path = path;
        }

        public async Task<Dictionary<string,string>> ReadStalkAsync()
        {
            using (StreamReader sr = File.OpenText(_path))
            {
                return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(sr.BaseStream);
            };
        }
    }
}