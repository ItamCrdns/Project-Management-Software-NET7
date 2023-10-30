using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class LatestStuffRepository : ILatestStuff
    {
        private readonly ApplicationDbContext _context;

        public LatestStuffRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LatestStuffDto> GetEntitiesCreatedLastWeek()
        {
            int projectsLastWeek = await _context.Projects.Where(p => p.Created >= DateTime.Now.AddDays(-7)).CountAsync();
            int tasksLastWeek = await _context.Tasks.Where(t => t.Created >= DateTime.Now.AddDays(-7)).CountAsync();
            int issuesLastWeek = await _context.Issues.Where(i => i.Created >= DateTime.Now.AddDays(-7)).CountAsync();

            LatestStuffDto latestStuffDto = new()
            {
                ProjectsLastWeek = projectsLastWeek,
                TasksLastWeek = tasksLastWeek,
                IssuesLastWeek = issuesLastWeek
            };

            return latestStuffDto;
        }
    }
}
