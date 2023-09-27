using CompanyPMO_.NET.Interfaces;
using System.Security.Claims;

namespace CompanyPMO_.NET.Repository
{
    public class UserIdentityRepository : IUserIdentity
    {
        public Task<int> GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdFromClaims = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userIdFromClaims is not null)
            {
                _ = int.TryParse(userIdFromClaims, out int userId);

                return Task.FromResult(userId);
            }

            return Task.FromResult(0);
        }
    }
}
