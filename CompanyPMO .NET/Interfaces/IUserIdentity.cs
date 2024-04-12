using System.Security.Claims;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IUserIdentity
    {
        int GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
