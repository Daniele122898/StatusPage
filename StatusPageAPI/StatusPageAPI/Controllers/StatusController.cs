using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatusPageAPI.Models;
using StatusPageAPI.Services;

namespace StatusPageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusService _ss;
        private readonly EntityConfigService _ecs;

        public StatusController(StatusService ss, EntityConfigService ecs)
        {
            _ss = ss;
            _ecs = ecs;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EntityStatus>> GetStatuses()
        {
            return Ok(_ss.GetStatuses());
        }

        [HttpGet("config")]
        public async Task<ActionResult<IEnumerable<EntityDeclaration>>> GetConfig()
        {
            var c = await _ecs.GetEntityDeclarationsAsync();
            return Ok(c);
        }
    }
}