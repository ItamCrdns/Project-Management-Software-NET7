namespace CompanyPMO_.NET.Dto
{
    public class TimelineDto
    {
        public int TimelineId { get; set; }
        public string Event { get; set; }
        public DateTime Created { get; set; }
        public int EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int? IssueId { get; set; }
        public string Type { get; set; }
        public EmployeeShowcaseDto Employee { get; set; }
        public ProjectDto? Project { get; set; }
        public TaskDto? Task { get; set; }
        public IssueDto? Issue { get; set; }
    }
}
