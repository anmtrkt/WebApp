using WebApp.DB;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Services.Identity
{
    public interface ITokenService
    {
        string CreateToken(User user, List<IdentityRole<Guid>> role);
    }
}
