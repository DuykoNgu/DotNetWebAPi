using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Repositories
{
    public interface ITokenRepo
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
