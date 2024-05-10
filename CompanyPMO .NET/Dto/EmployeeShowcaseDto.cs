namespace CompanyPMO_.NET.Dto
{
    public class EmployeeShowcaseDto
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime? LastLogin { get; set; }
        public TierDto Tier { get; set; }
        public string? Workload { get; set; }
    }
}
