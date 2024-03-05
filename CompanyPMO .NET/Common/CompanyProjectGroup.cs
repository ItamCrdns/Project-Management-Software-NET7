using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Common
{
    public class CompanyProjectGroup
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public bool IsCurrentUserInTeam { get; set; }
        public IEnumerable<ProjectDto> Projects { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public DateTime LatestProjectCreation { get; set; }
    }
}
