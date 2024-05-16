using CompanyPMO_.NET.Interfaces;
using System.Security.Claims;

namespace CompanyPMO_.NET.Services
{
    public class GetUserIdService : IUserIdentity
    {
        public int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdFromClaims = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromClaims is not null)
            {
                _ = int.TryParse(userIdFromClaims, out int userId);

                return userId;
            }

            return 0;
        }
    }
}
