using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;
        private readonly IJwt _jwtService;
        private readonly IProject _projectService;
        private readonly ITask _taskService;
        private readonly IIssue _issueService;
        private readonly IUserIdentity _userIdentityService;
        //private readonly Lazy<Task<int>> _lazyUserId;

        public EmployeeController(IEmployee employeeService, IJwt jwtService, IProject projectService, ITask taskService, IIssue issueService, IUserIdentity userIdentityService)
        {
            _employeeService = employeeService;
            _jwtService = jwtService;
            _projectService = projectService;
            _taskService = taskService;
            _issueService = issueService;
            _userIdentityService = userIdentityService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(LoginResponseDto))]
        public async Task<IActionResult> Login(EmployeeLoginDto employee)
        {
            var employeeAuthentication = await _employeeService.AuthenticateEmployee(employee.Username, employee.Password);

            if(employeeAuthentication.result.Authenticated)
            {
                var loggedEmployee = await _employeeService.GetEmployeeForClaims(employee.Username);

                var token = _jwtService.JwtTokenGenerator(loggedEmployee);

                //HttpContext.Response.Cookies.Append("JwtToken", token, new CookieOptions
                //{
                //    Expires = DateTime.Now.AddDays(7),
                //    HttpOnly = true,
                //    SameSite = SameSiteMode.Strict,
                //    Secure = true, // No HTTPS
                //    IsEssential = true
                //});

                var loginResponse = new LoginResponseDto
                {
                    Result = employeeAuthentication.result,
                    Message = employeeAuthentication.message,
                    Employee = employeeAuthentication.employee,
                    Token = token
                };

                return Ok(loginResponse);
            }
            else if(!employeeAuthentication.result.Authenticated)
            {
                var loginResponse = new LoginResponseDto
                {
                    Result = employeeAuthentication.result,
                    Message = employeeAuthentication.message,
                    Employee = employeeAuthentication.employee
                };

                return Unauthorized(loginResponse);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterEmployee([FromForm] EmployeeRegisterDto employee, [FromForm] IFormFile? profilePicture)
        {
            var (result, status) = await _employeeService.RegisterEmployee(employee, profilePicture);

            if (!status)
            {
                return BadRequest(new { Created = status, Message = "Employee could not be created" });
            }

            return Ok(new { Created = status, Message = result });
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            Employee employee = await _employeeService.GetEmployeeById(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("my-team")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesBySupervisorId([FromQuery] FilterParams filterParams)
        {
            int supervisorId = _userIdentityService.GetUserIdFromClaims(HttpContext.User);

            var employees = await _employeeService.GetEmployeesBySupervisorId(supervisorId, filterParams);

            if (employees == null || employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [HttpPost("logout")]
        [ProducesResponseType(204)]
        public IActionResult EmployeeLogout()
        {
            Response.Cookies.Delete("JwtToken");

            return NoContent();
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("username/{username}")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeeByUsername(string username)
        {
            EmployeeDto employee = await _employeeService.GetEmployeeByUsername(username);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/colleagues")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesWorkingInTheSameCompany(string username, int page, int pageSize)
        {
            var employees = await _employeeService.GetEmployeesWorkingInTheSameCompany(username, page, pageSize);

            if (employees == null || employees.Data == null || !employees.Data.Any())
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/colleagues/search/{employeeToSearch}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchEmployeesWorkingInTheSameCompany(string username, string employeeToSearch, int page, int pageSize)
        {
            var employees = await _employeeService.SearchEmployeesWorkingInTheSameCompany(employeeToSearch, username, page, pageSize);

            if (employees == null || employees.Data == null || !employees.Data.Any())
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesShowcasePaginated(int page, int pageSize)
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User);

            var employees = await _employeeService.GetEmployeesShowcasePaginated(employeeId, page, pageSize);

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("all/search/{employee}")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchEmployeesShowcasePaginated(string employee, int page, int pageSize)
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User);

            var employees = await _employeeService.SearchEmployeesShowcasePaginated(employeeId, employee, page, pageSize);

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/projects/all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectsByEmployeeUsername(string username, [FromQuery] FilterParams filterParams)
        {
            var projects = await _projectService.GetProjectsByEmployeeUsername(username, filterParams);

            if (projects == null || projects.Data == null || !projects.Data.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/projects/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectsShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var projects = await _projectService.GetProjectsShowcaseByEmployeeUsername(username, page, pageSize);

            if (projects == null || projects.Data == null || !projects.Data.Any())
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/tasks/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTasksShowcaseByEmployeeUsername(string username, int page, int pageSize)
        {
            var tasks = await _taskService.GetTasksShowcaseByEmployeeUsername(username, page, pageSize);

            if (tasks == null || tasks.Data == null || !tasks.Data.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/tasks/all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTasksByEmployeeUsername(string username, int page, int pageSize)
        {
            var tasks = await _taskService.GetTasksByEmployeeUsername(username, page, pageSize);

            if (tasks == null || tasks.Data == null || !tasks.Data.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("{username}/issues/showcase")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IssueShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetIssuesByEmployeeUsername(string username, int page, int pageSize)
        {
            var issues = await _issueService.GetIssuesShowcaseByEmployeeUsername(username, page, pageSize);

            if (issues == null || issues.Data == null || !issues.Data.Any())
            {
                return NotFound();
            }

            return Ok(issues);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me/tier")]
        [ProducesResponseType(200, Type = typeof(Employee))]
        public async Task<IActionResult> GetEmployeeByIdForClaims()
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User); // * Get the employee Id from the cookie

            TierDto tier = await _employeeService.GetEmployeeTier(employeeId);

            return Ok(tier);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me")]
        [ProducesResponseType(200, Type = typeof(EmployeeDto))]
        public async Task<IActionResult> GetMyEmployee()
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User); // * Get the employee Id from the cookie

            var employee = await _employeeService.GetEmployeeById(employeeId);

            return Ok(employee);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPatch("me/update")]
        [ProducesResponseType(200, Type = typeof(OperationResult<EmployeeShowcaseDto>))]
        [ProducesErrorResponseType(typeof(OperationResult<EmployeeShowcaseDto>))]
        public async Task<IActionResult> UpdateMyEmployee([FromForm] UpdateEmployeeDto employee, [FromForm] IFormFile? profilePicture, [FromForm] string? currentPassword)
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User); // * Get the employee Id from the cookie

            var result = await _employeeService.UpdateEmployee(employeeId, employee, profilePicture, currentPassword);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //[Authorize(Policy = "EmployeesAllowed")]
        //[HttpGet("{clientId}/projects/created")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        //public async Task<IActionResult> GetAndSearchEmployeesByProjectsCreatedInClient(string? employeeIds, int clientId, int page, int pageSize)
        //{
        //    var employees = await _employeeService.GetAndSearchEmployeesByProjectsCreatedInClient(employeeIds, clientId, page, pageSize);

        //    return Ok(employees);
        //}

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("employees/by-employee-ids")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeShowcaseDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesFromAListOfEmployeeIds([FromQuery] string? employeeIds)
        {
            // Make sure that the string you pass its in the following format: 1-2-3-4-5.
            var employees = await _employeeService.GetEmployeesFromAListOfEmployeeIds(employeeIds);

            if (!employees.Any())
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpPost("me/confirm-password")]
        [ProducesResponseType(200, Type = typeof(OperationResult<bool>))]
        public async Task<IActionResult> ConfirmPassword([FromBody] EmployeePasswordDto employee)
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User); // * Get the employee Id from the cookie

            var isPasswordCorrect = await _employeeService.ConfirmPassword(employeeId, employee.Password);

            if (isPasswordCorrect == null)
            {
                return NotFound();
            }

            return Ok(isPasswordCorrect);
        }

        [Authorize(Policy = "EmployeesAllowed")]
        [HttpGet("me/password-last-verification")]
        [ProducesResponseType(200, Type = typeof(OperationResult<DateTime>))]
        public async Task<IActionResult> PasswordLastVerification()
        {
            int employeeId = _userIdentityService.GetUserIdFromClaims(HttpContext.User); // * Get the employee Id from the cookie

            var lastVerification = await _employeeService.PasswordLastVerification(employeeId);

            return Ok(lastVerification);
        }
    }
}
