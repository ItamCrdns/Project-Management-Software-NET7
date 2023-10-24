using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Dto
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string? ProfilePicture { get; set; }
        public Tier Tier { get; set; }
        public EmployeeDto? Supervisor { get; set; }

        public int ProjectTotalCount { get; set; }
        public int ProjectsCreated { get; set; }
        public int ProjectsParticipant { get; set; }

        public int TaskTotalCount { get; set; }
        public int TasksCreated { get; set; }
        public int TasksParticipant { get; set; }

        public int IssueTotalCount { get; set; }
        public int IssuesCreated { get; set; }
        public int IssuesParticipant { get; set; }
    }
}
