using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using StatusPageAPI.Models;

namespace StatusPageAPI.Services
{
    public class EntityConfigService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
        private const int _LOCK_TIMEOUT_MS = 5000;
        
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
            try
            {
                if (!_semaphore.Wait(_LOCK_TIMEOUT_MS))
                    return null;

                string json = File.ReadAllText(_jsonPath);
                return JsonSerializer.Deserialize<List<EntityDeclaration>>(json, _jsonOp);                
            }
            finally
            {
                _semaphore.Release();
            }

        }
        
        public async Task<List<EntityDeclaration>> GetEntityDeclarationsAsync()
        {
            // Don't wanna read the file while its being written to
            // TODO make this more async and performant :)
            try
            {
                if (!await _semaphore.WaitAsync(_LOCK_TIMEOUT_MS))
                    return null;

                string json = await File.ReadAllTextAsync(_jsonPath);
                return JsonSerializer.Deserialize<List<EntityDeclaration>>(json, _jsonOp);     
            }
            finally
            {
                _semaphore.Release();
            }           
        }

        public void WriteEntityDeclarations(List<EntityDeclaration> entityDeclarations)
        {
            // Don't wanna write the file while its being read
            // TODO make this more async and performant :)
            try
            {
                if (!_semaphore.Wait(_LOCK_TIMEOUT_MS))
                    return;

                string json = JsonSerializer.Serialize(entityDeclarations, _jsonOp);
                File.WriteAllText(_jsonPath, json);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task WriteEntityDeclarationsAsync(List<EntityDeclaration> entityDeclarations)
        {
            // Don't wanna write the file while its being read
            // TODO make this more async and performant :)
            try
            {
                if (!await _semaphore.WaitAsync(_LOCK_TIMEOUT_MS))
                    return;

                string json = JsonSerializer.Serialize(entityDeclarations, _jsonOp);
                await File.WriteAllTextAsync(_jsonPath, json);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}