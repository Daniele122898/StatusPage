using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArgonautCore.Lw;
using Microsoft.AspNetCore.Hosting;
using StatusPageAPI.Models;

namespace StatusPageAPI.Services
{
    public class EntityConfigService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
        private const int _LOCK_TIMEOUT_MS = 5000;
        
        private readonly string _jsonPath;
        private readonly string _jsonPathCopy;
        private readonly JsonSerializerOptions _jsonOp = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true
        };
        
        public EntityConfigService(IWebHostEnvironment env)
        {
            _jsonPath = Path.Combine(env.ContentRootPath, "entities.json");
            _jsonPathCopy = Path.Combine(env.ContentRootPath, "entitiesCopy.json");
        }

        public async Task<Result<bool, Error>> TryEditConfig(string oldEntityId, EntityDeclaration newEntity)
        {
            try
            {
                if (!await _semaphore.WaitAsync(_LOCK_TIMEOUT_MS))
                    return new Result<bool, Error>(new Error("Failed to lock resource"));
                
                string json = await File.ReadAllTextAsync(_jsonPath);
                var entities =  JsonSerializer.Deserialize<List<EntityDeclaration>>(json, _jsonOp);

                var old = entities.FirstOrDefault(x => x.Identifier == oldEntityId);
                if (old == null)
                    return new Result<bool, Error>(new Error($"Entity with id {oldEntityId} could not be found"));

                entities.Remove(old);
                
                if (entities.Any(x => x.Identifier == newEntity.Identifier))
                    return new Result<bool, Error>(new Error($"Identifier {newEntity.Identifier} already exists in declarations"));
                
                entities.Add(newEntity);

                await this.WriteToCopyAndReplace(entities);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Result<bool, Error>> TryAddConfig(EntityDeclaration entity)
        {
            try
            {
                if (!await _semaphore.WaitAsync(_LOCK_TIMEOUT_MS))
                    return new Result<bool, Error>(new Error("Failed to lock resource"));
                
                string json = await File.ReadAllTextAsync(_jsonPath);
                var entities =  JsonSerializer.Deserialize<List<EntityDeclaration>>(json, _jsonOp);
                if (entities.Any(x => x.Identifier == entity.Identifier))
                    return new Result<bool, Error>(new Error($"Identifier {entity.Identifier} already exists in declarations"));
                
                entities.Add(entity);

                await this.WriteToCopyAndReplace(entities);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Result<bool, Error>> TryRemoveConfigWithId(string identifier)
        {
            try
            {
                if (!await _semaphore.WaitAsync(_LOCK_TIMEOUT_MS))
                    return new Result<bool, Error>(new Error("Failed to lock resource"));
                
                string json = await File.ReadAllTextAsync(_jsonPath);
                var entities =  JsonSerializer.Deserialize<List<EntityDeclaration>>(json, _jsonOp);
                if (entities.RemoveAll(x => x.Identifier == identifier) == 0)
                    return new Result<bool, Error>(new Error($"No entity found with identifier {identifier}"));
                
                await this.WriteToCopyAndReplace(entities);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task WriteToCopyAndReplace(List<EntityDeclaration> entities)
        {
            string json = JsonSerializer.Serialize(entities, _jsonOp);
            await File.WriteAllTextAsync(_jsonPathCopy, json);
            if (File.Exists(_jsonPath))
                File.Delete(_jsonPath);
            
            File.Move(_jsonPathCopy, _jsonPath);
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