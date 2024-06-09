using WebApp.DB;
using WebApp.Extensions;
using WebApp.Services.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.UsersModel;

namespace WebApp.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Context _context;
        private readonly ITokenService _token;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(RoleManager<IdentityRole<Guid>> roleManager, Context context, ITokenService token, IConfiguration configuration, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _token = token;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AuthResponse> Authenticate(User user)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var roleIds = await _context.UserRoles.Where(r => r.UserId == user.Id).Select(x => x.RoleId).ToListAsync();
            var roles = await _context.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

            var accessToken = _token.CreateToken(user, roles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

            await _context.SaveChangesAsync();
            return new AuthResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<object?> Registration(RegisterRequest request)
        {
            var user = new User
            {
                Name = request.FirstName + " " + request.LastName + " " + request.MiddleName,
                Email = request.Email,
                UserName = request.Email,
                BirthDate = request.BirthDate
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return null;

            var findUser =
                await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email)
                ??
                throw new Exception($"User {request.Email} not found");

            await _userManager.AddToRoleAsync(findUser, RoleConstants.Member);
            return new AuthenticationRequest
            {
                Email = request.Email,
                Password = request.Password
            };


        }

    }
}
        /// <summary>
        /// Подключить сваггер и доделать логин и регистрацию...
        /// 
        ///         почитать как выдать жвт-токен в кукис, сделать дату экспирации для куки, если прям много\
        ///         времени будет то можно будет сделать защищенный эндпоинт. задать вопросЫ по этому поводу.\
        ///         
        ///         
        ///         
        ///         в планах - 
        ///         1. заполнить БД отелями и номерами
        ///         2. зарегать пару юзеров
        ///         3. Доделать создание и проверку букингов, брать букинги пользователя сессии а не по емэйл, доработаь весь роутинг этого процесса\
        ///         доработать респонз и реквест модели, букинг сервис, букинг контроллер 
        ///         5. Настроить под себя регистрацию
        ///         6. как-то проверять букинги на давность и удалять старые через какое-то время.
        ///         7. отправлять на имейл письмо о создании букинга и напоминание о нём. хендфайр.
        ///         
        /// 
        /// </summary>
        /// <returns></returns>
