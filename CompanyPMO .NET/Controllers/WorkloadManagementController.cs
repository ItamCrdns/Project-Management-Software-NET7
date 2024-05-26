using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Interfaces.Workload_interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkloadManagementController : ControllerBase
    {
        private readonly IWorkloadOverdues _workloadOverdues;
        public WorkloadManagementController(IWorkloadOverdues workloadOverdues)
        {
            _workloadOverdues = workloadOverdues;
        }

        [HttpPost("projects/update")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> UpdateOverdueProjects()
        {
            var result = await _workloadOverdues.UpdateOverdueProjects();;

            return Ok(result);
        }

        [HttpPost("tasks/update")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> UpdateOverdueTasks()
        {
            var result = await _workloadOverdues.UpdateOverdueTasks();

            return Ok(result);
        }

        [HttpPost("issues/update")]
        [ProducesResponseType(200, Type = typeof(OperationResult))]
        public async Task<IActionResult> UpdateOverdueIssues()
        {
            var result = await _workloadOverdues.UpdateOverdueIssues();

            return Ok(result);
        }
    }
}
