using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using WebApp.DB;
using WebApp.Services.Identity;
using WebApp.DB;
using WebApp.Extensions;
using WebApp.Services.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.UsersModel;


namespace WebApp.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Context _context;
        private readonly ITokenService _token;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AdminService(RoleManager<IdentityRole<Guid>> roleManager, Context context, ITokenService token, IConfiguration configuration, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _token = token;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<bool> RoleMaking(Guid userId, int role)
        {
            var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (role == 1)
            {
                
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Administrator);
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Moderator);
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Member);
                await _userManager.AddToRoleAsync(findUser, RoleConstants.Administrator);
                await _context.SaveChangesAsync();
                return true;
            }
            else if (role == 2)
            {
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Administrator);
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Moderator);
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Member);
                await _userManager.AddToRoleAsync(findUser, RoleConstants.Moderator);
                await _context.SaveChangesAsync();
                return true;
            }
            else if (role == 3)
            {
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Administrator);
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Moderator);
                await _userManager.RemoveFromRoleAsync(findUser, RoleConstants.Member);
                await _userManager.AddToRoleAsync(findUser, RoleConstants.Member);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<List<UserModel>> AllUsers()
        {
            var AllUsers = await _context.Users.ToListAsync();
            var Response = AllUsers.Select(item => new UserModel
            {
                Id = item.Id,
                BirthDate = item.BirthDate,
                Name = item.Name,
                Password = item.PasswordHash,
                Email = item.Email,
                Sex = item.Sex

            }).ToList();
            return Response;
        }
        public async Task<object> OneUserById(Guid UserId)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (User != null)
            {
                return User;
            }
            else
            {
                return "Пользователь не найден";
            }
        }
        public async Task<object> OneUserByEmail(string Email)
        {
            var NormEmail = Email.ToLower().ToUpper();
            var User = await _context.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == NormEmail);
            if (User != null)
            {
                return User;
            }
            else
            {
                return "Пользователь не найден";
            }
        }
        public async Task<bool> DeleteUser(Guid UserId)
        {
            try
            {
                await _context.Users.Where(u => u.Id == UserId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
