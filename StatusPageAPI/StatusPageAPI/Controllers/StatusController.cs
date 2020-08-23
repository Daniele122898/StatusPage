using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusPageAPI.Dtos.StatusDtos;
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
        private readonly SpecialNoticeService _sns;

        public StatusController(StatusService ss, EntityConfigService ecs, SpecialNoticeService sns)
        {
            _ss = ss;
            _ecs = ecs;
            _sns = sns;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EntityStatus>> GetStatuses()
        {
            return Ok(_ss.GetStatuses());
        }

        [HttpGet("notice")]
        public ActionResult<SpecialNotice> GetSpecialNotice()
        {
            if (_sns.SpecialNotice == null)
                return NotFound();

            return _sns.SpecialNotice;
        }
        
        [HttpPost("notice")]
        [Authorize]
        public IActionResult SetSpecialNotice(SetSpecialNoticeDto specialNoticeDto)
        {
            _sns.SpecialNotice = new SpecialNotice(specialNoticeDto.Status, specialNoticeDto.Notice);
            return Ok();
        }
        
        [HttpDelete("notice")]
        [Authorize]
        public IActionResult RemoveSpecialNotice()
        {
            _sns.SpecialNotice = null;
            return Ok();
        }

        [HttpGet("config")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EntityDeclaration>>> GetConfig()
        {
            var c = await _ecs.GetEntityDeclarationsAsync();
            return Ok(c);
        }
    }
}