using System.Security.Claims;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IUserIdentity
    {
        Task<int> GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
