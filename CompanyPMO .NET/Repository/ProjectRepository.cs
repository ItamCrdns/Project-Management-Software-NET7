using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Repository
{
    public class ProjectRepository : IProject
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<Project> CreateProject(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
