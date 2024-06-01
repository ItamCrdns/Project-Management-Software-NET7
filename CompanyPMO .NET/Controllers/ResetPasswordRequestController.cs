using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordRequestController : ControllerBase
    {
        private readonly IResetPasswordRequest _resetPasswordRequestService;
        public ResetPasswordRequestController(IResetPasswordRequest resetPasswordRequestService)
        {
            _resetPasswordRequestService = resetPasswordRequestService;
        }

        [HttpGet("exists/{requestGuid}")]
        public async Task<IActionResult> RequestExists(Guid requestGuid)
        {
            var result = await _resetPasswordRequestService.RequestExists(requestGuid);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var result = await _resetPasswordRequestService.RequestPasswordReset(email);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordWithToken(string email, int token, string newPassword)
        {
            var result = await _resetPasswordRequestService.ResetPasswordWithToken(email, token, newPassword);

            return Ok(result);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var employeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var result = await _resetPasswordRequestService.ResetPasswordWithCurrentPassword(employeeId, currentPassword, newPassword);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("is-token-valid")]
        [ProducesResponseType(200, Type = typeof(OperationResult<bool>))]
        [ProducesResponseType(400, Type = typeof(OperationResult<bool>))]
        [ProducesResponseType(404, Type = typeof(OperationResult<bool>))]
        public async Task<IActionResult> IsTokenValid(int token, Guid requestGuid)
        {
            var result = await _resetPasswordRequestService.IsTokenValid(token, requestGuid);

            if (result.Message == "Employee not found.")
            {
                return NotFound(result);
            }

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
