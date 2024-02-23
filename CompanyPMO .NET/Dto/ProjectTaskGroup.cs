namespace CompanyPMO_.NET.Dto
{
    public class ProjectTaskGroup
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public bool IsCurrentUserOwner { get; set; }
        public bool IsCurrentUserInTeam { get; set; }
        public IEnumerable<TaskShowcaseDto> Tasks { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public DateTime LatestTaskCreation { get; set; }
    }

}
