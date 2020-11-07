using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace DemoGrpc.Web.Services
{
    public class ProtoService
    {
        private readonly string _baseDirectory;
        private readonly Dictionary<string, IEnumerable<string>> _protosByVersion;

        public ProtoService(IWebHostEnvironment webHost)
        {
            _baseDirectory = webHost.ContentRootPath;
            _protosByVersion = Get(_baseDirectory);
        }

        public async Task<string> Get()
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, _protosByVersion);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }

        }

        public string Get(int version, string protoName)
        {
            var filePath = $"{_baseDirectory}\\protos\\v{version}\\{protoName}";
            var exist = File.Exists(filePath);

            return exist ? filePath : null;
        }

        private Dictionary<string, IEnumerable<string>> Get(string baseDirectory) => 

            Directory.GetDirectories($"{baseDirectory}\\protos")
            .Select(x => new { version = x, protos = Directory.GetFiles(x).Select(Path.GetFileName)})
            .ToDictionary(o => Path.GetRelativePath("protos", o.version), o => o.protos);
        
    }
}