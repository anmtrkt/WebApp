using WebApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.UsersModel;

namespace WebApp.Services.UserServices
{

    public interface IUserService
    {
        Task<User?> GetUserByEmail(string email);
        Task<AuthResponse> Authenticate(User user);
        Task<object?> Registration(RegisterRequest request);

    }
}
