using Microsoft.AspNetCore.Identity;

namespace NZWalks3.API.Repository
{
    public interface ITokenRepository
    {
       string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
