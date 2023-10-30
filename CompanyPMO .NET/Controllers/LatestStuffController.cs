using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "SupervisorOnly")]
    public class LatestStuffController : ControllerBase
    {
        private readonly ILatestStuff _latestStuffService;

        public LatestStuffController(ILatestStuff latestStuffService)
        {
            _latestStuffService = latestStuffService;
        }

        [HttpGet("lastweek")]
        [ProducesResponseType(200, Type = typeof(LatestStuffDto))]
        public async Task<IActionResult> GetEntitiesCreatedLastWeek()
        {
            LatestStuffDto latestStuffDto = await _latestStuffService.GetEntitiesCreatedLastWeek();

            return Ok(latestStuffDto);
        }
    }
}
