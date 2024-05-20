namespace CompanyPMO_.NET.Dto
{
    public class TimelineDto
    {
        public int TimelineId { get; set; }
        public string Event { get; set; }
        public DateTime Created { get; set; }
        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public EmployeeShowcaseDto Employee { get; set; }
    }
}
