using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [AllowAnonymous]
        [HttpGet("is-token-valid")]
        public async Task<IActionResult> IsTokenValid(int token, Guid requestGuid)
        {
            var result = await _resetPasswordRequestService.IsTokenValid(token, requestGuid);

            return Ok(result);
        }
    }
}
