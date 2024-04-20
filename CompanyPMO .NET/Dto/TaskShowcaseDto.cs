namespace CompanyPMO_.NET.Dto
{
    public class TaskShowcaseDto
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int ProjectId { get; set; }
        public ProjectSomeInfoDto Project { get; set; }
        public EmployeeShowcaseDto TaskCreator { get; set; }
        public ICollection<EmployeeShowcaseDto> Employees { get; set; }
    }
}
