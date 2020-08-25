using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusPageAPI.Dtos.ConfigDtos;
using StatusPageAPI.Models;
using StatusPageAPI.Services;

namespace StatusPageAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly EntityConfigService _ecs;

        public ConfigController(EntityConfigService ecs)
        {
            _ecs = ecs;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntityDeclaration>>> GetConfig()
        {
            var c = await _ecs.GetEntityDeclarationsAsync();
            return Ok(c);
        }

        [HttpPost("{identifier}")]
        public async Task<IActionResult> EditConfig(string identifier, EntityDeclaration entity)
        {
            var res = await _ecs.TryEditConfig(identifier, entity);
            
            if (!res)
                return BadRequest(res.Err().Message.Get());

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddConfig(EntityConfigDto entity)
        {
            if (!entity.IsCategory && string.IsNullOrWhiteSpace(entity.HealthEndpoint.ToString()))
                return BadRequest("Health Endpoint cannot be null if entity is not a category.");
                
            var res = await _ecs.TryAddConfig(entity.ToDeclaration());
            if (!res)
                return BadRequest(res.Err().Message.Get());

            return Ok();
        }

        [HttpDelete("{identifier}")]
        public async Task<IActionResult> DeleteConfig(string identifier)
        {
            var res = await _ecs.TryRemoveConfigWithId(identifier);
            if (!res)
                return BadRequest(res.Err().Message.Get());
            
            return Ok();
        }
    }
}