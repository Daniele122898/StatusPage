using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using StatusPageAPI.Models;

namespace StatusPageAPI.Services
{
    public class EntityConfigService
    {

        private readonly object _lock = new object();
        private readonly string _jsonPath;
        private readonly JsonSerializerOptions _jsonOp = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true
        };
        
        public EntityConfigService(IWebHostEnvironment env)
        {
            _jsonPath = Path.Combine(env.ContentRootPath, "entities.json");
        }

        public List<EntityDeclaration> GetEntityDeclarations()
        {
            // Don't wanna read the file while its being written to
            // TODO make this more async and performant :)
            lock (_lock)
            {
                string json = File.ReadAllText(_jsonPath);
                return JsonSerializer.Deserialize<List<EntityDeclaration>>(json, _jsonOp);                
            }
        }

        public void WriteEntityDeclarations(List<EntityDeclaration> entityDeclarations)
        {
            // Don't wanna write the file while its being read
            // TODO make this more async and performant :)
            lock (_lock)
            {
                string json = JsonSerializer.Serialize(entityDeclarations, _jsonOp);
                File.WriteAllText(_jsonPath, json);
            }
        }
    }
}