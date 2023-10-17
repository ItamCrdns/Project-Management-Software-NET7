namespace CompanyPMO_.NET.Dto
{
    public class TaskShowcaseDto
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? StartedWorking { get; set; }
        public DateTimeOffset? Finished { get; set; }

        public ICollection<EmployeeShowcaseDto>? Employees { get; set; }
    }
}
