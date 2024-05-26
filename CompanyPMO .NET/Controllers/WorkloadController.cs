using CompanyPMO_.NET.Interfaces.Workload_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkloadController : ControllerBase
    {
        private readonly IWorkloadEmployee _workload;
        public WorkloadController(IWorkloadEmployee workload)
        {
            _workload = workload;
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}")]
        [ProducesResponseType(200, Type = typeof(Workload))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetWorkloadByEmployee(string username)
        {
            var workload = await _workload.GetWorkloadByEmployee(username);

            if (workload is null)
            {
                return NotFound();
            }

            return Ok(workload);
        }
    }
}
