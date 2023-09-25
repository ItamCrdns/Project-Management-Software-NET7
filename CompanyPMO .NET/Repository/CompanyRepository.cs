using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Repository
{
    public class CompanyRepository : ICompany
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company> GetCompany(int companyId) => await _context.Companies.FindAsync(companyId);
    }
}
