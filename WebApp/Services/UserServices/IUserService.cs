using WebApp.DB;
using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Services.UserServices
{

    public interface IUserService
    {
        Task<User?> GetUserByEmail(string email);
        Task<AuthResponse> Authenticate(User user);
        Task<object?> Registration(RegisterRequest request);
    }
}
